using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Conquer.Game.Messages;

public record MsgRegister : IMessage
{
    private const ushort MaleAvatar = 1;
    private const ushort FemaleAvatar = 201;
    private const ushort StartingHair = 310;
    private const int HairColors = 7;
    private const uint StartingMoney = 100;

    private static readonly ushort[] Bodies = [1003, 1004, 2001, 2002];
    private static readonly Profession[] Professions = [Profession.InternTrojan, Profession.InternWarrior, Profession.InternArcher, Profession.InternTaoist];
    private static readonly string[] Reserved = ["GM", "gm", "PM", "pm", "SYSTEM", "system", "NPC", "npc"];

    public string? UserName { get; set; }
    public string? PlayerName { get; set; }
    public string? Password { get; set; }
    public ushort Model { get; set; }
    public Profession Profession { get; set; }
    public uint Id { get; set; }
    public ushort Size => 60;
    public MessageType Type => MessageType.MsgRegister;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        UserName = buffer[4..20].ReadString();
        PlayerName = buffer[20..36].ReadString();
        Password = buffer[36..52].ReadString();
        Model = ReadUInt16LittleEndian(buffer[52..]);
        Profession = (Profession)ReadUInt16LittleEndian(buffer[54..]);
        Id = ReadUInt32LittleEndian(buffer[56..]);
    }

    public void Write(Span<byte> buffer)
    {
        buffer[4..20].WriteString(UserName);
        buffer[20..36].WriteString(PlayerName);
        buffer[36..52].WriteString(Password);
        WriteUInt16LittleEndian(buffer[52..], Model);
        WriteUInt16LittleEndian(buffer[54..], (ushort)Profession);
        WriteUInt32LittleEndian(buffer[56..], Id);
    }

    public async Task HandleAsync(GameClient client, GameStore store, GameData data)
    {
        if (client.UserId is null || client.Player is not null || !Bodies.Contains(Model) || !Professions.Contains(Profession) ||
            data.PointAllots.GetValueOrDefault(((byte)((int)Profession / 10), (byte)1)) is not { } allot ||
            await store.HasCharacterAsync(client.UserId))
        {
            client.Write(MsgTalk.System("Invalid user.", TalkChannel.Register));
            return;
        }

        if (!IsValid(PlayerName))
        {
            client.Write(MsgTalk.System("Invalid name.", TalkChannel.Register));
            return;
        }

        if (await store.IsNameTakenAsync(PlayerName))
        {
            client.Write(MsgTalk.System("Name taken.", TalkChannel.Register));
            return;
        }

        var player = new Player
        {
            UserId = client.UserId,
            Name = PlayerName,
            Model = Model,
            Avatar = Model / 1000 == 1 ? MaleAvatar : FemaleAvatar,
            Hair = (ushort)(StartingHair + Random.Shared.Next(HairColors) * 100),
            PkMode = PkMode.Peace,
            Money = StartingMoney,
            Level = 1,
            Strength = allot.Strength,
            Agility = allot.Agility,
            Vitality = allot.Vitality,
            Spirit = allot.Spirit,
            Profession = Profession,
            MapId = Player.BirthPoint.Map,
            X = Player.BirthPoint.X,
            Y = Player.BirthPoint.Y,
            RecordMapId = Player.BirthPoint.Map,
            RecordX = Player.BirthPoint.X,
            RecordY = Player.BirthPoint.Y
        };
        player.Health = player.BaseMaxHealth;
        player.Mana = (ushort)player.BaseMaxMana;

        try
        {
            await store.AddAsync(player);
        }
        catch (DbUpdateException)
        {
            client.Write(MsgTalk.System("Name taken.", TalkChannel.Register));
            return;
        }

        client.Write(MsgTalk.System("ANSWER_OK", TalkChannel.Register));
    }

    private static bool IsValid([NotNullWhen(true)] string? name) =>
        name is { Length: > 0 and <= Player.MaxNameLength } && name != Player.Unmarried && Player.HasAllowedCharacters(name) &&
        !Reserved.Any(name.Contains);
}