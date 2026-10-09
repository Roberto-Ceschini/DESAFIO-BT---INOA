using System.Net.Http;

public class ApiService
{
    private readonly HttpClient client;
    private readonly string baseUrl;
    public ApiService(HttpClient httpClient, ApiSettings settings)
    {
        client = httpClient;
        baseUrl = settings.BaseUrl;

        AuthorizationHeader(settings.Token);
    }

    public async Task<string> GetStockQuoteAsync(string symbol)
    {
        var response = await client.GetAsync($"{baseUrl}{symbol}");
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response: {content}");
        return content;
    }

    private void AuthorizationHeader(string token)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
}