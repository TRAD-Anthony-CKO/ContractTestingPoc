// ReSharper disable ConvertToPrimaryConstructor
namespace ServiceA.Tests;

using System.Net;
using System.Text.Json;
using Humanizer;
using OneOf;

public sealed class ApiClient : IAsyncLifetime
{
    private readonly HttpClient _client;

    public ApiClient(HttpClient client) => _client = client;

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        _client.Dispose();
        return Task.CompletedTask;
    }

    public async Task<ApiResponse<TResponse>> Get<TResponse>(string url)
        where TResponse : class
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
        var responseMessage = await _client.SendAsync(httpRequest);
        return await SendRequest<TResponse>(responseMessage);
    }

    private static async Task<ApiResponse<TResponse>> SendRequest<TResponse>(HttpResponseMessage responseMessage)
        where TResponse : class
    {
        var apiResponseJson = await responseMessage.Content.ReadAsStringAsync();

        var result = new ApiResponse<TResponse> { StatusCode = responseMessage.StatusCode };

        if (apiResponseJson.Length <= 0)
        {
            return result;
        }

        try
        {
            if (apiResponseJson.Contains(nameof(ValidationError.ErrorCodes).Underscore()))
            {
                throw new NotSupportedException();
            }

            result.Response = JsonSerializer.Deserialize<TResponse>(apiResponseJson) ?? throw new NotSupportedException();
        }
        catch (Exception e) when (e is JsonException or NotSupportedException)
        {
            result.Response = JsonSerializer.Deserialize<ValidationError>(apiResponseJson)!;
        }

        return result;
    }
}

public sealed class ApiResponse<T> where T : class
{
    public OneOf<T, ValidationError> Response { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public sealed class ValidationError
{
    public string RequestId { get; set; } = default!;
    public string ErrorType { get; set; } = default!;
    public string[] ErrorCodes { get; set; } = default!;
}