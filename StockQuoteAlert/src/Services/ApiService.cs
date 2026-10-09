using System.Text.Json;

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

    public async Task<StockQuoteResponse> GetStockQuoteAsync(string symbol)
    {
        var response = await client.GetAsync($"{baseUrl}{symbol}");
        response.EnsureSuccessStatusCode();//A IA sugeriu essa linha para garantir que a resposta seja bem sucedida

        string content = await response.Content.ReadAsStringAsync();
        
        return FilterGetResponse(content);
    }

    private static StockQuoteResponse FilterGetResponse(string jsonContent)
    {

        BrapiApiResponse apiResponse = JsonSerializer.Deserialize<BrapiApiResponse>(jsonContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
        ?? throw new Exception("Falha ao desserializar a resposta da API");

        BrapiQuoteResult result = apiResponse.Results?.FirstOrDefault() ?? 
        throw new Exception("Nenhuma cotação encontrada");

        BrapiQuoteData data = result.Data 
        ?? throw new Exception("Dados da cotação não encontrados na resposta da API.");

        
        return new StockQuoteResponse
        {
            Symbol = result.Symbol,
            RegularMarketPrice = data.RegularMarketPrice,
            RegularMarketTime = data.RegularMarketTime
        };//Essa linha foi auxiliada pela IA pois a classe StockQuoteResponse nao tinha construtor e eu nao sabia como passar os parametros com c#
       
    }

    private void AuthorizationHeader(string token)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
}