public class Stock
{
    public string Symbol { get; }
    private readonly List<Quote> quotes = new();

    public Stock(string symbol)
    {
        Symbol = symbol;
    }

    public void AddQuote(decimal price, DateTime date)
    {
        Quote quote = new(price, date);
        quotes.Add(quote);
    }

}