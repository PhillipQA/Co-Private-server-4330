using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using static System.Buffers.Binary.BinaryPrimitives;

namespace Conquer.Protocols;

public class SizeTypePrefixedProtocol(MessageService messageService) : IProtocol
{
    public const int MaximumMessageSize = 1024;

    private const int HeaderSize = 4;

    public bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed,
        ref SequencePosition examined, [NotNullWhen(true)] out IMessage? message)
    {
        message = null;
        var reader = new SequenceReader<byte>(input);

        if (!reader.TryReadLittleEndian(out short size) || !reader.TryReadLittleEndian(out short type))
        {
            return false;
        }

        if (size is < HeaderSize or > MaximumMessageSize)
        {
            throw new InvalidDataException($"Invalid message size {size}.");
        }

        if (input.Length < size)
        {
            return false;
        }

        consumed = examined = input.GetPosition(size);
        message = messageService.Create((MessageType)type);

        if (message is null)
        {
            return false;
        }

        var packet = input.Slice(0, size);

        if (packet.IsSingleSegment)
        {
            message.Read(packet.FirstSpan);
        }
        else
        {
            Span<byte> buffer = stackalloc byte[size];
            packet.CopyTo(buffer);
            message.Read(buffer);
        }

        return true;
    }

    public void WriteMessage(IMessage message, IBufferWriter<byte> output)
    {
        var size = message.Size;
        var buffer = output.GetSpan(size)[..size];
        buffer.Clear();
        WriteUInt16LittleEndian(buffer, size);
        WriteUInt16LittleEndian(buffer[2..], (ushort)message.Type);
        message.Write(buffer);
        output.Advance(size);
    }
}
