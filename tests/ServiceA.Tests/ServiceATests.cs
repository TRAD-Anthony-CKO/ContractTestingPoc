// ReSharper disable ConvertToPrimaryConstructor
namespace ServiceA.Tests;

using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceA.Tests.Internal;
using ServiceA.Tests.Pacts;
using ServiceB.SDK;
using Xunit.Abstractions;

public class ServiceATests
{
    private readonly ServiceBPact _serviceBPact;
    
    public ServiceATests(ITestOutputHelper output)
    {
        //TODO: pass the factory from IClassFixture for example
        _serviceBPact = new PactDefinition
        {
            ConsumerName = "Service A API",
            ProviderName = "Service B API"
        }.CreatePact<ServiceBPact, IServiceBClient>(null!, output);
    }

    // TODO: ValidateAsync is ugly with this design, pact-net don't expose the actual mock. Thinking of a clever way to fix this.
        // TODO: Service A might be dependant on Service B,C,D..., ValidateAsync might get in the way.
    // TODO: ApplicationFactory is created twice, figure out why.
    [Fact]
    public Task ServiceA_ReturnAValidResponse_WhileServiceB_IsValid()
        => _serviceBPact.ValidBehavior()
            .ValidateAsync(async factory =>
            {
                var apiClient = factory.CreateClient();
                var serviceAClient = new ApiClient(apiClient);
                var response= await serviceAClient.Get<string>("http://localhost:5276/a");

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Response.AsT0.Should().Be("Hello From Actual Service B");
            });
    
    [Fact(Skip = "This test will fail because ServiceB is not valid. Remove Skip to see the failure.")]
    public Task TestWillFailEvenIfServiceA_ReturnAValidResponse_BecauseServiceBIsNotValid()
        => _serviceBPact
            .ValidBehavior()
            .InvalidBehavior()
            .ValidateAsync(async factory =>
            {
                var serviceAClient = new ApiClient(factory.CreateClient());
                var response= await serviceAClient.Get<string>("http://localhost:5276/a");

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Response.AsT0.Should().Be("Hello From Actual Service B");
            });
}