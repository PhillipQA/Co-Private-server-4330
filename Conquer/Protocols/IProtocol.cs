using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace Conquer.Protocols;

public interface IProtocol
{
    bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed, ref SequencePosition examined,
        [NotNullWhen(true)] out IMessage? message);

    void WriteMessage(IMessage message, IBufferWriter<byte> output);
}