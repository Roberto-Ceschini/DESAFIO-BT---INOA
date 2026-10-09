public class Quote
{
    public decimal RegularMarketPrice { get; }
    public DateTime RegularMarketTime { get; }

    public Quote(decimal regularMarketPrice, DateTime regularMarketTime)
    {
        ValidatePrice(regularMarketPrice);

        RegularMarketPrice = regularMarketPrice;
        RegularMarketTime = regularMarketTime;
    }

    private void ValidatePrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException("O preço da cotação não deve ser negativo."); //Usei IA para indicar a excecão mais adequada nesse caso.
        }
    }
}