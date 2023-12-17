namespace ServiceB;

public class Startup
{
    public Startup(IConfiguration _)
    {
    }

    public void ConfigureServices(IServiceCollection services)
    {
    }

    public void Configure(IApplicationBuilder app, 
        IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(e
            => e.MapGet("/", () => TypedResults.Ok("Hello From Actual Service B")));
    }
}