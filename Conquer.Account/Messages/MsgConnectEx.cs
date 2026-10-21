using System.Net;

namespace Conquer.Account.Messages;

public enum RejectionCode : uint
{
    InvalidAccount = 1,
    ServerDown = 10,
    TryAgainLater = 11,
    Banned = 12,
    ServerBusy = 20,
    ServerFull = 21,
    UnknownError = 999
}

public record MsgConnectEx : IMessage
{
    public MsgConnectEx()
    {
    }

    public MsgConnectEx(ulong token, IPAddress ipAddress, int port)
    {
        Id = (uint)token;
        Data = (uint)(token >> 32);
        Info = ipAddress.ToString();
        Port = (ushort)port;
    }

    public MsgConnectEx(RejectionCode code)
    {
        Data = (uint)code;
        Info = code switch
        {
            RejectionCode.InvalidAccount => "ÕÊºÅÃû»ò¿ÚÁî´í",
            RejectionCode.ServerDown => "·þÎñÆ÷Î´Æô¶¯",
            RejectionCode.TryAgainLater => "ÇëÉÔºóÖØÐÂµÇÂ¼",
            RejectionCode.Banned => "¸ÃÕÊºÅ±»·âºÅ",
            RejectionCode.ServerBusy => "·þÎñÆ÷Ã¦ÇëÉÔºò",
            RejectionCode.ServerFull => "·þÎñÆ÷ÈËÊýÒÑÂú",
            _ => null
        };
    }

    public uint Id { get; set; }
    public uint Data { get; set; }
    public string? Info { get; set; }
    public ushort Port { get; set; }
    public ushort Size => 32;
    public MessageType Type => MessageType.MsgConnectEx;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Data = ReadUInt32LittleEndian(buffer[8..]);
        Info = buffer[12..28].ReadString();
        Port = (ushort)ReadUInt32LittleEndian(buffer[28..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Data);
        buffer[12..28].WriteString(Info);
        WriteUInt32LittleEndian(buffer[28..], Port);
    }
}