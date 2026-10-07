public class StockQuote
{
    private string name;
    private decimal price;
    private DateTime date;

    public StockQuote(string name, decimal price, DateTime date)
    {
        this.name = name;
        this.price = price;
        this.date = date;
    }

}