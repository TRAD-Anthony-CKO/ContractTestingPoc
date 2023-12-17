// ReSharper disable UnusedMethodReturnValue.Global
namespace ServiceB.SDK;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceBClient(this IServiceCollection services, string url)
    {
        services.AddHttpClient<IServiceBClient, ServiceBClient>(nameof(IServiceBClient),x=> x.BaseAddress = new Uri(url));
        return services;
    }
}