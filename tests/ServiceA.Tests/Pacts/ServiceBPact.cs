// ReSharper disable ConvertToPrimaryConstructor
// ReSharper disable UnusedMethodReturnValue.Global
namespace ServiceA.Tests.Pacts;

using System.Net;
using PactNet;
using ServiceA.Tests.Internal;
using ServiceB.SDK;

public class ServiceBPact : ConsumerPact<IServiceBClient>
{
    public ServiceBPact ValidBehavior()
    {
        PactBuilder.UponReceiving("A very valid downstream behavior")
            .WithRequest(HttpMethod.Get, "/")
            .WillRespond()
            .WithJsonBody("Hello From Actual Service B")
            .WithStatus(HttpStatusCode.OK);

        return this;
    }
    
    public ServiceBPact InvalidBehavior()
    {
        PactBuilder.UponReceiving("Behavior 2")
            .WithRequest(HttpMethod.Get, "/api/orders/201")
            .WithHeader("Accept", "application/json")
            .WillRespond()
            .WithJsonBody("Pact implementation for Behavior 2")
            .WithStatus(HttpStatusCode.NotFound);

        return this;
    }

    public ServiceBPact(IPactBuilderV4 pact, ApplicationFactory factory) : base(pact, factory)
    {
    }
}