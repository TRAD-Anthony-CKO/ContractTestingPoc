using Microsoft.AspNetCore.Mvc;
using ServiceB.SDK;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServiceBClient("http://localhost:9011");

var app = builder.Build();

app.MapGet("/a", async ([FromServices] IServiceBClient client) => Results.Ok(await client.GetIdAsync()));

app.Run();