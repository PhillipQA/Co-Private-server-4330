using System.IO.Pipelines;
using Conquer.Cryptography;

namespace Conquer.Middleware;

internal sealed class EncryptingPipeWriter(PipeWriter inner, IConnectionCipher cipher) : PipeWriter
{
    private Memory<byte> _memory;

    public override Memory<byte> GetMemory(int sizeHint = 0) => _memory = inner.GetMemory(sizeHint);

    public override Span<byte> GetSpan(int sizeHint = 0) => GetMemory(sizeHint).Span;

    public override void Advance(int bytes)
    {
        var span = _memory.Span[..bytes];
        cipher.Encrypt(span, span);
        _memory = _memory[bytes..];
        inner.Advance(bytes);
    }

    public override ValueTask<FlushResult> FlushAsync(CancellationToken cancellationToken = default) =>
        inner.FlushAsync(cancellationToken);

    public override void CancelPendingFlush() => inner.CancelPendingFlush();

    public override void Complete(Exception? exception = null) => inner.Complete(exception);
}
