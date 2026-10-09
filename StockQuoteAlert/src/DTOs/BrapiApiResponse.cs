//Este codigo foi gerado por IA apenas para deixar mais rapido 
// o desenvolvimento e a desserialização da resposta da Brapi
public class BrapiApiResponse
{
    public List<BrapiQuoteResult> Results { get; set; } = new();
}

public class BrapiQuoteResult
{
    public string Symbol { get; set; } = "";
    public BrapiQuoteData? Data { get; set; }
}

public class BrapiQuoteData
{
    public decimal RegularMarketPrice { get; set; }
    public DateTime RegularMarketTime { get; set; }
}