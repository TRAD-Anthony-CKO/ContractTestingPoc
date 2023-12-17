// ReSharper disable ConvertToPrimaryConstructor

namespace ServiceA.Tests.Internal;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PactNet;
using ServiceB.SDK;

public abstract class ConsumerPact<TClient> where TClient : class
{
    protected readonly IPactBuilderV4 PactBuilder;
    private WebApplicationFactory<Program> _factory;

    public async Task ValidateAsync(Func<PactApplicationFactory<TClient>, Task> testLogic)
    {
        await PactBuilder.VerifyAsync(async consumerContext =>
        {
            var appWithPact= new PactApplicationFactory<TClient>(consumerContext.MockServerUri);
            await testLogic(appWithPact);
        });
    }

    protected ConsumerPact(IPactBuilderV4 pact, ApplicationFactory factory)
    {
        PactBuilder = pact;
        _factory = factory;
    }
}

public class PactDefinition
{
    public string ConsumerName { get; set; } = default!;
    public string ProviderName { get; set; } = default!;
}

public class PactApplicationFactory<TClient> : ApplicationFactory where TClient: class
{
    private readonly Uri _mockServerUri;

    public PactApplicationFactory(Uri mockServerUri) => _mockServerUri = mockServerUri;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            //TODO: Find a better way to override the HttpClient. This removes all consumers
            services.RemoveAll<HttpClient>()
                .RemoveAll<IHttpClientFactory>()
                .RemoveAll<TClient>()
                //TODO: Fix this ugly hack
                .AddServiceBClient(_mockServerUri.AbsoluteUri);
        });
    } 
}