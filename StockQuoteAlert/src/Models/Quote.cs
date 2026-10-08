public class Quote
{
    public decimal Price { get; }
    public DateTime Date { get; }

    public Quote(decimal price, DateTime date)
    {
        ValidatePrice(price);

        Price = price;
        Date = date;
    }

    private void ValidatePrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException("O preço da cotação não deve ser negativo."); //Usei IA para indicar a excecão mais adequada nesse caso.
        }
    }
}