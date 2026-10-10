public class MonitoringService
{
    private readonly ApiService apiService;
    private readonly EmailService emailService;

    public MonitoringService(ApiService apiService, EmailService emailService)
    {
        this.apiService = apiService;
        this.emailService = emailService;
    }

    public async Task MonitorStockQuoteAsync(Stock stock)
    {
        while (true)
        {
            StockQuoteResponse response = await apiService.GetStockQuoteAsync(stock.Symbol);

            stock.AddQuote(response.RegularMarketPrice, response.RegularMarketTime);

            HandleSellAlert(stock, response);
            HandleBuyAlert(stock, response);

            await Task.Delay(5000);
        }
    }

    private void HandleSellAlert(Stock stock, StockQuoteResponse response)
    {
        if (response.RegularMarketPrice < stock.SellPrice) return;

        emailService.SendSellAlert(stock, response);        
    }

    private void HandleBuyAlert(
        Stock stock,
        StockQuoteResponse response)
    {
        if (response.RegularMarketPrice > stock.BuyPrice) return;

        emailService.SendBuyAlert(stock, response);
    }
}