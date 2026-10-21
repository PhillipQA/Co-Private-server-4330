using System.Drawing;

namespace Conquer.Game.Messages;

public enum WeatherType : uint
{
    None = 1,
    Rain = 2,
    Snow = 3,
    Sand = 4,
    AutumnLeaves = 5,
    CherryBlossomPetals = 7,
    CherryBlossomPetalsWind = 8,
    BlowingCotton = 9,
    Atoms = 10,
    Cloudy = 11
}

public record MsgWeather : IMessage
{
    public static MsgWeather Fine => new() { WeatherType = WeatherType.None, Color = Color.Empty };

    public WeatherType WeatherType { get; set; }
    public uint Intensity { get; set; }
    public uint Direction { get; set; }
    public Color Color { get; set; } = Color.White;
    public ushort Size => 20;
    public MessageType Type => MessageType.MsgWeather;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        WeatherType = (WeatherType)ReadUInt32LittleEndian(buffer[4..]);
        Intensity = ReadUInt32LittleEndian(buffer[8..]);
        Direction = ReadUInt32LittleEndian(buffer[12..]);
        Color = Color.FromArgb(ReadInt32LittleEndian(buffer[16..]));
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], (uint)WeatherType);
        WriteUInt32LittleEndian(buffer[8..], Intensity);
        WriteUInt32LittleEndian(buffer[12..], Direction);
        WriteInt32LittleEndian(buffer[16..], Color.FromArgb(0, Color).ToArgb());
    }
}