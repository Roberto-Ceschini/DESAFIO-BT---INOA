public class Stock
{
    public string Name { get; }
    private readonly List<Quote> quotes = new();

    public Stock(string name)
    {
        Name = name;
    }

    public void AddQuote(decimal price, DateTime date)
    {
        Quote quote = new(price, date);
        quotes.Add(quote);
    }

}