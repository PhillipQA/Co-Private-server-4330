using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Conquer;

public class MessageService
{
    private readonly Dictionary<MessageType, Func<IMessage>> _factories = [];
    private readonly Dictionary<MessageType, BoundHandler> _handlers = [];
    private readonly ILogger<MessageService> _logger;
    private readonly IServiceProvider _services;

    public MessageService(IServiceProvider services, ILogger<MessageService> logger, Type clientType)
    {
        _services = services;
        _logger = logger;

        var isService = services.GetRequiredService<IServiceProviderIsService>();

        foreach (var type in clientType.Assembly.GetTypes())
        {
            if (!type.IsAssignableTo(typeof(IMessage)) || !Enum.TryParse<MessageType>(type.Name, out var messageType))
            {
                continue;
            }

            _factories[messageType] = Expression.Lambda<Func<IMessage>>(Expression.New(type)).Compile();

            if ((type.GetMethod("Handle") ?? type.GetMethod("HandleAsync")) is { } method)
            {
                _handlers[messageType] = new(method, clientType, isService);
            }
        }
    }

    public IMessage? Create(MessageType messageType)
    {
        if (_factories.TryGetValue(messageType, out var factory))
        {
            return factory();
        }

        _logger.WarnOnce("Message not found for {MessageType}.", messageType);
        return null;
    }

    public async Task HandleAsync(Client client, IMessage message)
    {
        if (!_handlers.TryGetValue(message.Type, out var handler))
        {
            _logger.WarnOnce("Message handler not found for {MessageType}.", message.Type);
            return;
        }

        try
        {
            await handler.InvokeAsync(_services, client, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception handling {Message} from {Client}.", message, client);
        }
    }
}
