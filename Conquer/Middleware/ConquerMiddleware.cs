using System.IO.Pipelines;
using Conquer.Cryptography;
using Conquer.Protocols;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Conquer.Middleware;

public static class ConquerMiddlewareExtensions
{
    public static TBuilder UseConquer<TBuilder>(this TBuilder builder) where TBuilder : IConnectionBuilder
    {
        builder.Use(next => new ConquerMiddleware(next, builder.ApplicationServices).OnConnectionAsync);
        return builder;
    }
}

internal class ConquerMiddleware(ConnectionDelegate next, IServiceProvider serviceProvider)
{
    public async Task OnConnectionAsync(ConnectionContext connection)
    {
        var cipher = serviceProvider.GetRequiredService<IConnectionCipher>();
        var logger = serviceProvider.GetRequiredService<ILogger<ConquerMiddleware>>();

        connection.Features.Set(cipher);
        connection.Features.Set(serviceProvider.GetRequiredService<IProtocol>());
        connection.Features.Set<ILogger>(serviceProvider.GetRequiredService<ILogger<Client>>());

        var transport = connection.Transport;
        await using var decrypting = new DecryptingPipeReader(transport.Input, cipher, logger);
        connection.Transport = new DuplexPipe(decrypting.Reader, new EncryptingPipeWriter(transport.Output, cipher));

        try
        {
            await next(connection);
        }
        finally
        {
            connection.Transport = transport;
        }
    }

    private sealed record DuplexPipe(PipeReader Input, PipeWriter Output) : IDuplexPipe;
}

internal sealed class DecryptingPipeReader : IAsyncDisposable
{
    private readonly Pipe _pipe = new();
    private readonly Task _pump;
    private readonly PipeReader _source;

    public DecryptingPipeReader(PipeReader source, IConnectionCipher cipher, ILogger logger)
    {
        _source = source;
        _pump = PumpAsync(cipher, logger);
    }

    public PipeReader Reader => _pipe.Reader;

    public async ValueTask DisposeAsync()
    {
        _source.CancelPendingRead();
        await _pipe.Reader.CompleteAsync();
        await _pump;
    }

    private async Task PumpAsync(IConnectionCipher cipher, ILogger logger)
    {
        try
        {
            while (true)
            {
                var result = await _source.ReadAsync();

                foreach (var segment in result.Buffer)
                {
                    cipher.Decrypt(segment.Span, _pipe.Writer.GetSpan(segment.Length));
                    _pipe.Writer.Advance(segment.Length);
                }

                _source.AdvanceTo(result.Buffer.End);

                var flush = await _pipe.Writer.FlushAsync();

                if (result.IsCanceled || result.IsCompleted || flush.IsCompleted)
                {
                    break;
                }
            }
        }
        catch (Exception ex) when (ex is ConnectionResetException or ConnectionAbortedException)
        {
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected exception while decrypting.");
        }

        await _pipe.Writer.CompleteAsync();
    }
}
