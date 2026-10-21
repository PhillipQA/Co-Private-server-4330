using Conquer.Cryptography;
using Conquer.Protocols;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Conquer;

public static class ConquerServiceCollectionExtensions
{
    public static IServiceCollection AddConquer<TClient>(this IServiceCollection services) where TClient : Client =>
        services
            .AddTransient<IConnectionCipher, TqConnectionCipher>()
            .AddTransient<IProtocol, SizeTypePrefixedProtocol>()
            .AddSingleton(provider => new MessageService(provider,
                provider.GetRequiredService<ILogger<MessageService>>(), typeof(TClient)));
}
