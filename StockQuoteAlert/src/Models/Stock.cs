public class Stock
{
    public string Symbol { get; }
    public decimal SellPrice { get; private set; }
    public decimal BuyPrice { get; private set; }

    private readonly List<Quote> quotes = new();

    public Stock(string symbol, decimal sellPrice, decimal buyPrice)
    {
        ValidatePrices(sellPrice, buyPrice);

        Symbol = symbol;
        SellPrice = sellPrice;
        BuyPrice = buyPrice;
    }

    public void UpdatePrices(decimal sellPrice, decimal buyPrice)
    {
        ValidatePrices(sellPrice, buyPrice);

        SellPrice = sellPrice;
        BuyPrice = buyPrice;
    }

    public void AddQuote(decimal price, DateTime date)
    {
        Quote quote = new Quote(price, date);

        quotes.Add(quote);
    }

    private static void ValidatePrices(decimal sellPrice, decimal buyPrice)
    {
        if (sellPrice <= buyPrice)
        {
            throw new ArgumentException(
                "O preço de venda deve ser maior que o preço de compra."
            ); //Usei IA para indicar a excecão mais adequada nesse caso.
        }
    }
}