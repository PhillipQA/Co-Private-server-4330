using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Conquer;

public static class LoggerExtensions
{
    private static readonly ConcurrentDictionary<(string Message, object Value), byte> Warned = [];

    extension(ILogger logger)
    {
        public void WarnOnce(string message, object value) =>
            logger.Log(Warned.TryAdd((message, value), 0) ? LogLevel.Warning : LogLevel.Debug, message, value);
    }
}
