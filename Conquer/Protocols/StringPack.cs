using System.Text;

namespace Conquer.Protocols;

public static class StringPack
{
    private static readonly Encoding Encoding = Encoding.Latin1;

    public static int Size(List<string> values)
    {
        var size = 1;

        foreach (var value in values)
        {
            size += 1 + Encoding.GetByteCount(value);
        }

        return size;
    }

    extension(ReadOnlySpan<byte> buffer)
    {
        public string ReadString()
        {
            var end = buffer.IndexOf((byte)0);
            return Encoding.GetString(end < 0 ? buffer : buffer[..end]);
        }

        public List<string> ReadStrings()
        {
            var values = new List<string>(buffer[0]);
            buffer = buffer[1..];

            while (values.Count < values.Capacity)
            {
                values.Add(Encoding.GetString(buffer.Slice(1, buffer[0])));
                buffer = buffer[(1 + buffer[0])..];
            }

            return values;
        }
    }

    extension(Span<byte> buffer)
    {
        public void WriteString(string? value) => Encoding.GetBytes(value, buffer);

        public void WriteStrings(List<string> values)
        {
            buffer[0] = (byte)values.Count;
            buffer = buffer[1..];

            foreach (var value in values)
            {
                buffer[0] = (byte)Encoding.GetBytes(value, buffer[1..]);
                buffer = buffer[(1 + buffer[0])..];
            }
        }
    }
}
