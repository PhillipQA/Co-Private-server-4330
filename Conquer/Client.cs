using System.IO.Pipelines;
using System.Net;
using Conquer.Cryptography;
using Conquer.Protocols;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace Conquer;

public abstract partial class Client(ConnectionContext connection)
{
    private static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(10);

    private readonly ILogger _logger = connection.Features.GetRequiredFeature<ILogger>();
    private readonly IProtocol _protocol = connection.Features.GetRequiredFeature<IProtocol>();
    private readonly Lock _writeLock = new();
    private bool _closed;
    private Task _flush = Task.CompletedTask;
    private readonly CancellationToken _shutdown = ShutdownRequested(connection);
    private readonly CancellationTokenSource _reading = CancellationTokenSource.CreateLinkedTokenSource(ShutdownRequested(connection));

    private PipeReader Input => connection.Transport.Input;
    private PipeWriter Output => connection.Transport.Output;

    public bool IsClosed => _closed || connection.ConnectionClosed.IsCancellationRequested;
    public EndPoint RemoteEndPoint => connection.RemoteEndPoint!;
    public IConnectionCipher ConnectionCipher { get; } = connection.Features.GetRequiredFeature<IConnectionCipher>();
    public async ValueTask<IMessage?> ReadAsync()
    {
        try
        {
            while (!IsClosed)
            {
                _reading.CancelAfter(IdleTimeout);
                var result = await Input.ReadAsync(_reading.Token);
                var buffer = result.Buffer;
                var consumed = buffer.Start;
                var examined = buffer.End;

                try
                {
                    if (result.IsCanceled)
                    {
                        break;
                    }

                    if (_protocol.TryParseMessage(buffer, ref consumed, ref examined, out var message))
                    {
                        LogReceived(_logger, message, this);
                        return message;
                    }
                }
                finally
                {
                    Input.AdvanceTo(consumed, examined);
                }

                if (result.IsCompleted)
                {
                    break;
                }
            }
        }
        catch (OperationCanceledException) when (_shutdown.IsCancellationRequested)
        {
            _closed = true;
        }
        catch (OperationCanceledException)
        {
            Drop("Idle timeout exceeded.");
        }
        catch (Exception ex) when (ex is ConnectionResetException or ConnectionAbortedException)
        {
            _closed = true;
        }
        catch (Exception ex)
        {
            Drop($"Unreadable message: {ex.Message}");
        }

        _reading.Dispose();
        return null;
    }
    public void Write(IMessage message)
    {
        lock (_writeLock)
        {
            if (IsClosed)
            {
                return;
            }

            if (!_flush.IsCompleted)
            {
                Drop("Send buffer full.");
                return;
            }

            try
            {
                _protocol.WriteMessage(message, Output);

                var flush = Output.FlushAsync();
                _flush = flush.IsCompletedSuccessfully ? Task.CompletedTask : flush.AsTask();
            }
            catch (InvalidOperationException)
            {
                _closed = true;
                return;
            }
        }

        LogSent(_logger, message, this);
    }
    public void Close()
    {
        lock (_writeLock)
        {
            _closed = true;
        }

        Input.CancelPendingRead();
    }

    protected void Drop(string reason)
    {
        _closed = true;
        _logger.LogInformation("Dropping {Client}: {Reason}", this, reason);
        connection.Abort(new ConnectionAbortedException(reason));
    }

    public override string? ToString() => RemoteEndPoint.ToString();

    private static CancellationToken ShutdownRequested(ConnectionContext connection) =>
        connection.Features.Get<IConnectionLifetimeNotificationFeature>()?.ConnectionClosedRequested ?? CancellationToken.None;

    [LoggerMessage(LogLevel.Debug, "Received {Message} from {Client}.")]
    private static partial void LogReceived(ILogger logger, IMessage message, Client client);

    [LoggerMessage(LogLevel.Debug, "Sent {Message} to {Client}.")]
    private static partial void LogSent(ILogger logger, IMessage message, Client client);
}
