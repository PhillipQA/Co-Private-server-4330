namespace Conquer.Game.Messages;

public enum ActionType : ushort
{
    GetPosition = 74,
    GetItemSet = 75,
    GetGoodFriend = 76,
    GetWeaponSkillSet = 77,
    GetMagicSet = 78,
    ChangeDirection = 79,
    Emotion = 81,
    ChangeMap = 85,
    EnterMap = 86,
    LevelUp = 92,
    ClearXp = 93,
    Revive = 94,
    DeleteCharacter = 95,
    SetPkMode = 96,
    GetGuildInfo = 97,
    Mine = 99,
    BotCheckA = 100,
    TeamMemberPos = 101,
    QueryPlayer = 102,
    AbortMagic = 103,
    MapColor = 104,
    QueryTeamMember = 106,
    DropMagic = 109,
    DropSkill = 110,
    CreateBooth = 111,
    DestroyBooth = 114,
    PostCommand = 116,
    QueryEquipment = 117,
    AbortTransform = 118,
    TakeOff = 120,
    GetMoney = 121,
    QueryEnemyInfo = 123,
    OpenDialog = 126,
    Transfer = 129,
    LoginCompleted = 130,
    KickBack = 108,
    LeaveMap = 132,
    Jump = 133,
    Ghost = 137,
    Synchro = 138,
    QueryFriendInfo = 140,
    QueryOfflineMessage = 141,
    ChangeFace = 142,
    ScoreCounter = 1000,
    AddScore = 1001
}

public record MsgAction : IMessage
{
    public uint Timestamp { get; set; } = (uint)Environment.TickCount;
    public uint Id { get; set; }

    public uint Data { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }

    public ushort DataX
    {
        get => (ushort)Data;
        set => Data = (Data & 0xFFFF0000) | value;
    }

    public ushort DataY
    {
        get => (ushort)(Data >> 16);
        set => Data = (Data & 0x0000FFFF) | ((uint)value << 16);
    }

    public ushort Direction { get; set; }
    public ActionType Action { get; set; }

    public ushort Size => 24;
    public MessageType Type => MessageType.MsgAction;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Timestamp = ReadUInt32LittleEndian(buffer[4..]);
        Id = ReadUInt32LittleEndian(buffer[8..]);
        Data = ReadUInt32LittleEndian(buffer[12..]);
        X = ReadUInt16LittleEndian(buffer[16..]);
        Y = ReadUInt16LittleEndian(buffer[18..]);
        Direction = ReadUInt16LittleEndian(buffer[20..]);
        Action = (ActionType)ReadUInt16LittleEndian(buffer[22..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Timestamp);
        WriteUInt32LittleEndian(buffer[8..], Id);
        WriteUInt32LittleEndian(buffer[12..], Data);
        WriteUInt16LittleEndian(buffer[16..], X);
        WriteUInt16LittleEndian(buffer[18..], Y);
        WriteUInt16LittleEndian(buffer[20..], Direction);
        WriteUInt16LittleEndian(buffer[22..], (ushort)Action);
    }

    public async Task HandleAsync(Player player, Map map, World world, GameStore store, GuildService guilds, ILogger<MsgAction> logger)
    {
        if (player.Pet is { } pet && pet.Id == Id)
        {
            if (Action is ActionType.Jump or ActionType.Transfer)
            {
                pet.Leap(DataX, DataY, this);
            }
            else if (Action != ActionType.Emotion)
            {
                logger.WarnOnce("Unhandled action {Action}.", Action);
            }

            return;
        }

        if (World.IsPetId(Id))
        {
            return;
        }

        if (Action is not (ActionType.QueryFriendInfo or ActionType.QueryEnemyInfo or ActionType.QueryPlayer or ActionType.ClearXp or
            ActionType.TeamMemberPos or ActionType.QueryEquipment))
        {
            player.Target = null;
        }

        switch (Action)
        {
            case ActionType.GetPosition:
                var current = world.Maps.GetValueOrDefault(player.MapId);
                Id = player.Id;
                Data = current?.DocId ?? player.MapId;
                X = player.X;
                Y = player.Y;
                Direction = (ushort)player.Direction;
                player.Send(this);

                if (current is not null)
                {
                    player.Send(new MsgMapInfo(current));
                }
                break;

            case ActionType.GetItemSet:
                foreach (var item in player.Carried)
                {
                    player.Send(new MsgItemInfo(item, ItemInfoAction.AddItem));
                }

                player.Send(this);
                break;

            case ActionType.GetGoodFriend:
                foreach (var friend in player.Friends)
                {
                    player.Send(new MsgFriend(friend.FriendPlayerId, FriendAction.GetInfo, world.Players.ContainsKey(friend.FriendPlayerId), friend.Name));
                }

                foreach (var enemy in player.Enemies)
                {
                    player.Send(new MsgFriend(enemy.EnemyPlayerId, FriendAction.EnemyAdd, world.Players.ContainsKey(enemy.EnemyPlayerId), enemy.Name));
                }

                player.NotifyFriends(true);
                player.Send(this);
                await ReadOfflineMessage();
                break;

            case ActionType.GetWeaponSkillSet:
                foreach (var weaponSkill in player.KnownWeaponSkills)
                {
                    player.Send(new MsgWeaponSkill(weaponSkill));
                }

                player.Send(this);
                break;

            case ActionType.GetMagicSet:
                foreach (var magic in player.KnownMagics)
                {
                    player.Send(new MsgMagicInfo(magic));
                }

                player.Send(this);
                break;

            case ActionType.ChangeDirection:
                player.Direction = (Direction)Direction;
                map.Broadcast(player, this);
                break;

            case ActionType.Emotion:
                Data = player.Emote(Data, world.UtcNow);
                map.Broadcast(player, this);
                break;

            case ActionType.ChangeMap:
                if (!player.IsAlive)
                {
                    break;
                }

                if (map.PortalBeside(player) is { } portal && world.Maps.TryGetValue(portal.MapId, out var destination))
                {
                    map.Transfer(player, destination, portal.PortalX, portal.PortalY);
                    break;
                }

                logger.LogWarning("{Player} took a portal at map {Map} ({X},{Y}) that leads nowhere.", player.Name, map.Id, player.X, player.Y);
                map.Transfer(player, world.Maps[Player.LoginPoint.Map], Player.LoginPoint.X, Player.LoginPoint.Y);
                player.Client.Close();
                break;

            case ActionType.DeleteCharacter:
                if (Data != player.StoragePin)
                {
                    break;
                }

                await guilds.OnDeletingAsync(player);
                player.OnDeleting();
                world.Leave(player);
                await store.DeleteAsync(player);
                player.Client.Close();
                break;

            case ActionType.SetPkMode:
                player.PkMode = (PkMode)Data;
                player.Interrupt();
                player.Send(this);
                break;

            case ActionType.GetGuildInfo:
                guilds.SendGuildInfo(player);
                guilds.Greet(player);
                player.Send(this);
                break;

            case ActionType.CreateBooth:
                if (player.Id != Id)
                {
                    break;
                }

                player.Direction = (Direction)Direction;

                if (Booth.Open(player) is { } booth)
                {
                    player.Send(new MsgAction { Id = player.Id, Data = booth.Id, Action = ActionType.CreateBooth });
                }
                break;

            case ActionType.DestroyBooth:
                player.Booth?.Close();
                player.Send(this);
                break;

            case ActionType.QueryEnemyInfo:
                if (world.Players.TryGetValue(Data, out var enemyInfo))
                {
                    player.Send(new MsgFriendInfo(enemyInfo, isEnemy: true));
                }
                break;

            case ActionType.LoginCompleted:
                if (player.Map is null && world.Maps.TryGetValue(player.MapId, out var start))
                {
                    player.Arrive(start, world.UtcNow);
                }
                break;

            case ActionType.QueryFriendInfo:
                if (world.Players.TryGetValue(Data, out var friendInfo))
                {
                    player.Send(new MsgFriendInfo(friendInfo));
                }
                break;

            case ActionType.Jump:
                if (player.Id != Id || player.X != X || player.Y != Y)
                {
                    player.KickBack();
                    break;
                }

                player.Jump(DataX, DataY, this);
                break;

            case ActionType.Synchro:
                if (player.Id != Id || player.X != DataX || player.Y != DataY)
                {
                    player.KickBack();
                }
                break;

            case ActionType.Mine:
                if (player.Id == Id)
                {
                    player.Mine(world.UtcNow);
                }
                break;

            case ActionType.ChangeFace:
                if (player.ChangeAvatar(DataX))
                {
                    map.Broadcast(player, this);
                    player.Team?.Send(this, player);
                }
                break;

            case ActionType.Revive:
                player.Revive(world.UtcNow);
                break;

            case ActionType.Ghost:
                if (player.BecomeGhost())
                {
                    player.Show(AttributeType.Look, player.Look);
                }
                break;

            case ActionType.ClearXp:
                if (Id == player.Id)
                {
                    player.SetXp(0);
                }
                break;

            case ActionType.QueryPlayer:
                if (map.Find<Entity>(Data) is { } other && player.CanSee(other))
                {
                    player.Send(other.Spawn);
                }
                break;

            case ActionType.QueryTeamMember:
                if (world.Players.GetValueOrDefault(Data) is { } member && member.MapId == player.MapId)
                {
                    player.Send(new MsgAction
                    {
                        Id = Id,
                        Data = member.Id,
                        X = member.X,
                        Y = member.Y,
                        Action = ActionType.QueryTeamMember
                    });
                }
                break;

            case ActionType.QueryEquipment:
                foreach (var item in world.Players.GetValueOrDefault(Data)?.Equipment ?? [])
                {
                    player.Send(new MsgItemInfo(item, ItemInfoAction.OtherPlayerEquipment) { ItemId = Data });
                }
                break;

            case ActionType.AbortTransform:
                player.Transform(null);
                break;

            case ActionType.TakeOff:
                player.ClearEffect(PlayerStatus.Flying);
                break;

            case ActionType.QueryOfflineMessage:
                await ReadOfflineMessage();
                break;

            default:
                logger.WarnOnce("Unhandled action {Action}.", Action);
                break;
        }

        async Task ReadOfflineMessage()
        {
            if (await store.TakeOfflineMessageAsync(player.Id) is { } word)
            {
                player.Send(new MsgTalk(TalkChannel.OfflineMessage, word.Sender, player.Name, word.SentAt.ToString(OfflineMessage.Stamp), word.Words));
            }
        }
    }
}