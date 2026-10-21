namespace Conquer.Game.Messages;

public record MsgMapInfo : IMessage
{
    public MsgMapInfo()
    {
    }

    public MsgMapInfo(Map map)
    {
        MapId = map.Id;
        DocId = map.DocId;
        MapType = (uint)map.Info.Type;
    }

    public uint MapId { get; set; }
    public uint DocId { get; set; }
    public uint MapType { get; set; }

    public ushort Size => 16;
    public MessageType Type => MessageType.MsgMapInfo;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        MapId = ReadUInt32LittleEndian(buffer[4..]);
        DocId = ReadUInt32LittleEndian(buffer[8..]);
        MapType = ReadUInt32LittleEndian(buffer[12..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], MapId);
        WriteUInt32LittleEndian(buffer[8..], DocId);
        WriteUInt32LittleEndian(buffer[12..], MapType);
    }
}