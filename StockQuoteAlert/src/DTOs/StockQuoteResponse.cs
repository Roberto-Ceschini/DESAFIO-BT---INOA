public class StockQuoteResponse
{
    public string Symbol { get; set; } = "";
    public decimal RegularMarketPrice { get; set; }
    public DateTime RegularMarketTime { get; set; }
}