namespace ServiceB.SDK;

using System.Text.Json;

public interface IServiceBClient
{
    public Task<string> GetIdAsync(CancellationToken? token = null);
}

public class ServiceBClient : IServiceBClient
{
    private readonly HttpClient _client;

    public ServiceBClient(HttpClient client) => _client = client;
    
    public async Task<string> GetIdAsync(CancellationToken? token = null)
    { 
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, "/");
        var responseMessage = await _client.SendAsync(httpRequest);

        var responseJson = await responseMessage.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<string>(responseJson) ?? string.Empty;
    }
}