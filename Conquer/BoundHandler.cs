using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Conquer;

public sealed class BoundHandler
{
    private delegate Task Invoker(Client client, object? instance, object?[] context, IServiceProvider services);

    private static readonly MethodInfo GetRequiredService =
        typeof(ServiceProviderServiceExtensions).GetMethod(nameof(ServiceProviderServiceExtensions.GetRequiredService),
            [typeof(IServiceProvider)])!;

    private static readonly ParameterExpression ClientParam = Expression.Parameter(typeof(Client));
    private static readonly ParameterExpression InstanceParam = Expression.Parameter(typeof(object));
    private static readonly ParameterExpression ContextParam = Expression.Parameter(typeof(object[]));
    private static readonly ParameterExpression ServicesParam = Expression.Parameter(typeof(IServiceProvider));

    private readonly Invoker _invoke;
    private bool _usesServices;

    public BoundHandler(MethodInfo method, Type clientType, IServiceProviderIsService isService,
        params Type[] contextTypes)
    {
        var client = Expression.Convert(ClientParam, clientType);

        var arguments = method.GetParameters().Select(parameter => parameter.ParameterType).Select(Expression (type) =>
        {
            if (type.IsAssignableFrom(clientType))
            {
                return client;
            }

            if (clientType.GetProperties().FirstOrDefault(property => property.PropertyType == type) is { } property)
            {
                return Expression.Property(client, property);
            }

            if (Array.FindIndex(contextTypes, context => type.IsAssignableFrom(context) || context.IsAssignableFrom(type)) is >= 0 and var index)
            {
                return Expression.Convert(Expression.ArrayIndex(ContextParam, Expression.Constant(index)), type);
            }

            if (!isService.IsService(type))
            {
                throw new InvalidOperationException(
                    $"{method.DeclaringType!.Name}.{method.Name} asks for {type.Name}, which is neither on {clientType.Name} nor a registered service.");
            }

            _usesServices = true;
            return Expression.Call(GetRequiredService.MakeGenericMethod(type), ServicesParam);
        }).ToArray();

        var instance = method.IsStatic ? null : Expression.Convert(InstanceParam, method.DeclaringType!);
        Expression call = Expression.Call(instance, method, arguments);

        if (method.ReturnType == typeof(void))
        {
            call = Expression.Block(call, Expression.Constant(Task.CompletedTask));
        }

        _invoke = Expression.Lambda<Invoker>(call, ClientParam, InstanceParam, ContextParam, ServicesParam).Compile();
    }

    public async Task InvokeAsync(IServiceProvider services, Client client, object? instance, params object?[] context)
    {
        if (!_usesServices)
        {
            await _invoke(client, instance, context, services);
            return;
        }

        await using var scope = services.CreateAsyncScope();
        await _invoke(client, instance, context, scope.ServiceProvider);
    }
}
