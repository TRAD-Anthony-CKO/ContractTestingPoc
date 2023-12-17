namespace ServiceA.Tests;

using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;

public class ApplicationFactory : WebApplicationFactory<Program>
{
    public ApplicationFactory() => Randomizer.Seed = new Random(8675309);
}