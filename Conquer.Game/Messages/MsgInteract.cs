namespace Conquer.Game.Messages;

public enum InteractAction
{
    Steal = 1,
    Attack = 2,
    Heal = 3,
    Poison = 4,
    Assassinate = 5,
    Freeze = 6,
    Unfreeze = 7,
    Propose = 8,
    Marry = 9,
    Divorce = 10,
    PresentMoney = 11,
    PresentItem = 12,
    SendFlowers = 13,
    Kill = 14,
    JoinGuild = 15,
    AcceptGuildMember = 16,
    KickOutGuildMember = 17,
    PresentPower = 18,
    QueryInfo = 19,
    RushAttack = 20,
    MagicAttack = 21,
    AbortMagic = 22,
    ReflectWeapon = 23,
    Bump = 24,
    Shoot = 25,
    ReflectMagic = 26
}

public record MsgInteract : IMessage
{
    public uint Timestamp { get; set; } = (uint)Environment.TickCount;
    public uint Sender { get; set; }
    public uint Target { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public InteractAction Action { get; set; }
    public uint Data { get; set; }
    public ushort Size => 28;
    public MessageType Type => MessageType.MsgInteract;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Timestamp = ReadUInt32LittleEndian(buffer[4..]);
        Sender = ReadUInt32LittleEndian(buffer[8..]);
        Target = ReadUInt32LittleEndian(buffer[12..]);
        X = ReadUInt16LittleEndian(buffer[16..]);
        Y = ReadUInt16LittleEndian(buffer[18..]);
        Action = (InteractAction)ReadUInt32LittleEndian(buffer[20..]);
        Data = ReadUInt32LittleEndian(buffer[24..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Timestamp);
        WriteUInt32LittleEndian(buffer[8..], Sender);
        WriteUInt32LittleEndian(buffer[12..], Target);
        WriteUInt16LittleEndian(buffer[16..], X);
        WriteUInt16LittleEndian(buffer[18..], Y);
        WriteUInt32LittleEndian(buffer[20..], (uint)Action);
        WriteUInt32LittleEndian(buffer[24..], Data);
    }

    public void Handle(Player player, Map map, World world, ILogger<MsgInteract> logger)
    {
        if (player.Pet is { } pet && pet.Id == Sender)
        {
            if (Action is InteractAction.Attack or InteractAction.Shoot or InteractAction.RushAttack)
            {
                pet.Target = map.Find<Fighter>(Target);
            }

            return;
        }

        if (World.IsPetId(Sender))
        {
            return;
        }

        switch (Action)
        {
            case InteractAction.Attack or InteractAction.Shoot:
                if (!player.SyncPosition(X, Y))
                {
                    player.Client.Close();
                    return;
                }

                player.Interrupt();
                player.ClearEffect(PlayerStatus.Invisible);

                if (Action == player.AttackKind)
                {
                    player.Target = map.Find<Fighter>(Target);
                    player.Tick(world.UtcNow);
                }
                break;

            case InteractAction.MagicAttack:
                CastMagic(player, map, world.Data, world.UtcNow);
                break;

            case InteractAction.Propose:
                Pledge(player.Propose);
                break;

            case InteractAction.Marry:
                Pledge(player.Marry);
                break;

            default:
                logger.WarnOnce("Unhandled action {Action}.", Action);
                break;
        }

        void Pledge(Func<Player, bool> vow)
        {
            if (map.Find<Player>(Target) is { } other)
            {
                vow(other);
            }
        }
    }

    private void CastMagic(Player player, Map map, GameData data, DateTimeOffset now)
    {
        Target = ((((Target & 0xFFFFE000) >> 13) | ((Target & 0x1FFF) << 19)) ^ 0x5F2D2463 ^ Sender) - 0x746F4AE6;
        X = (ushort)(X ^ (Sender & 0xFFFF) ^ 0x2ED6);
        X = (ushort)(((X << 1) | ((X & 0x8000) >> 15)) - 0x22EE);
        Y = (ushort)(Y ^ (Sender & 0xFFFF) ^ 0xB99B);
        Y = (ushort)(((Y << 5) | ((Y & 0xF800) >> 11)) - 0x8922);
        Data = (ushort)(Data ^ 0x915D ^ Sender);
        Data = (ushort)(((Data << 3) | (Data >> 13)) - 0xEB42);

        player.ClearEffect(PlayerStatus.Invisible);

        if (player.KnownMagic((ushort)Data) is not { } magic ||
            !data.MagicTypes.TryGetValue((magic.Type, magic.Level), out var spell) || spell.AutoActive != 0)
        {
            player.Interrupt();
            return;
        }

        if (spell.Sort != MagicSort.Dashing)
        {
            player.StartCast(spell, Target == 0 ? null : map.Find<Fighter>(Target), X, Y, now);
            return;
        }

        var direction = (Direction)(Target % 8);
        var (dx, dy) = direction.Step();

        if ((X, Y) != (player.X, player.Y) || !map.IsWalkable(X + dx, Y + dy))
        {
            player.Interrupt();
            player.KickBack();
            return;
        }

        player.Direction = direction;
        player.StartCast(spell, null, X, Y, now);
    }
}