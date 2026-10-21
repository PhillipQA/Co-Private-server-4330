namespace Conquer.Game.Messages;

public enum NpcAction : ushort
{
    Activate = 0,
    AddNpc = 1,
    LeaveMap = 2,
    DeleteNpc = 3,
    ChangePos = 4,
    LayNpc = 5
}

public record MsgNpc : IMessage
{
    public uint Id { get; set; }
    public uint Data { get; set; }
    public NpcAction Action { get; set; }
    public ushort NpcType { get; set; }
    public ushort Size => 16;
    public MessageType Type => MessageType.MsgNpc;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Data = ReadUInt32LittleEndian(buffer[8..]);
        Action = (NpcAction)ReadUInt16LittleEndian(buffer[12..]);
        NpcType = ReadUInt16LittleEndian(buffer[14..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Data);
        WriteUInt16LittleEndian(buffer[12..], (ushort)Action);
        WriteUInt16LittleEndian(buffer[14..], NpcType);
    }

    public void Handle(Player player, Map map, ScriptService scripts)
    {
        if (!player.IsAlive)
        {
            player.Tell("You are already dead.", TalkChannel.System);
            return;
        }

        if (map.Find<Entity>(Id) is not { } npc || npc is not (Npc or DynaNpc) || !player.CanSee(npc))
        {
            return;
        }

        if (Action == NpcAction.DeleteNpc)
        {
            if (npc is DynaNpc { IsDeletable: true } laid && laid.IsOwnedBy(player) && laid.Contents.Count == 0)
            {
                player.World.DeleteNpc(laid);
            }

            return;
        }

        scripts.OnTalk(player, npc);
    }
}