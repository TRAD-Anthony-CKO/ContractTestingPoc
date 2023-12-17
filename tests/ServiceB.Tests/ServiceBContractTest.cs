// ReSharper disable ConvertToPrimaryConstructor
namespace ServiceB.Tests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Output.Xunit;
using PactNet.Verifier;
using Xunit.Abstractions;

public class ServiceBContractTest : IDisposable
{
    private static readonly Uri ProviderUri = new("http://localhost:5224");

    private const string PactPath = "../../../../pacts/";
    private readonly PactVerifier _verifier;
    private readonly IHost _testServer;
    public ServiceBContractTest(ITestOutputHelper output)
    {
        _testServer = StartServer();

        _verifier = new PactVerifier("Service B API", new PactVerifierConfig
        {
            LogLevel = PactLogLevel.Debug,
            Outputters = new List<IOutput>
            {
                new XunitOutput(output)
            }
        });
    }

    [Fact]
    public void Verify()
    {
        var pactPath = Path.Combine(PactPath, "Service A API-Service B API.json");

        _verifier
            .WithHttpEndpoint(ProviderUri)
            .WithFileSource(new FileInfo(pactPath))
            .Verify();
    }

    private static IHost StartServer()
    {
        var server = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(ProviderUri.ToString());
                webBuilder.UseStartup<Startup>();
            })
            .Build();

        server.Start();
        return server;
    }

    public void Dispose()
    {
        _verifier.Dispose();
        _testServer.Dispose();
    }
}