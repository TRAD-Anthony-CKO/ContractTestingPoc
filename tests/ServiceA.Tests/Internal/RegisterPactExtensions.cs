// ReSharper disable HeapView.ObjectAllocation

namespace ServiceA.Tests.Internal;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Output.Xunit;
using Xunit.Abstractions;

public static class RegisterPactExtensions
{
    public static T CreatePact<T, T1>(this PactDefinition definition, ApplicationFactory factory, ITestOutputHelper output)
        where T : ConsumerPact<T1> where T1 : class
    {
        var config = new PactConfig
        {
            PactDir = "../../../../pacts/",
            Outputters = new[] { new XunitOutput(output) },
            DefaultJsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] { new StringEnumConverter() }
            },
            LogLevel = PactLogLevel.Debug
        };
        
        var pactBuilder = Pact.V4(definition.ConsumerName, definition.ProviderName, config).WithHttpInteractions();
        return (T)Activator.CreateInstance(typeof(T), pactBuilder, factory)!;
    }
}