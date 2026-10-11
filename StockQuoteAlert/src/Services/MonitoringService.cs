public class MonitoringService
{
    private const int MaxApiAttempts = 3;//Linha gerada por IA
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(5); //Linnha gerada por IA

    private readonly ApiService apiService;
    private readonly EmailService emailService;
    private enum PriceZone
    {
        Unknown, //O valor Unkown foi uma sugestão da IA, para resolver o problema da primeira execução do monitoramento.
        Buy,
        Neutral,
        Sell
    }

    public MonitoringService(ApiService apiService, EmailService emailService)
    {
        this.apiService = apiService;
        this.emailService = emailService;
    }

     public async Task MonitorStockQuoteAsync(Stock stock)
    {
        PriceZone previousZone = PriceZone.Unknown;

        while (true)
        {
            // Fetch the stock quote from the API
            StockQuoteResponse response = await GetStockQuoteWithRetryAsync(stock.Symbol);

            decimal currentPrice = response.RegularMarketPrice;

            // Add the quote to the stock's history
            stock.AddQuote(currentPrice, response.RegularMarketTime);

            // Determine the current price zone based on the stock's buy and sell prices
            PriceZone currentZone = GetPriceZone(stock, currentPrice);

            // Handle price zone changes and send email alerts if necessary
            HandlePriceZoneChange(stock, response, previousZone, currentZone);

            // Update the previous zone for the next iteration
            previousZone = currentZone;

            await Task.Delay(5000);
        }
    }

    //O código da função abaixo foi gerado pela IA como auxilio na para melhorar a robustez do monitoramento, com tentativas de reconexão em caso de falha na API.
    private async Task<StockQuoteResponse> GetStockQuoteWithRetryAsync(string symbol)
    {
        int attempt = 1;

        while (true)
        {
            try
            {
                return await apiService.GetStockQuoteAsync(symbol);
            }
            catch (HttpRequestException) when (attempt < MaxApiAttempts)
            {
                Console.WriteLine(
                    $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] Falha ao consultar {symbol}. " +
                    $"Nova tentativa em {RetryDelay.TotalSeconds} segundos " +
                    $"({attempt}/{MaxApiAttempts})."
                );

                attempt++;
                await Task.Delay(RetryDelay);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Não foi possível obter a cotação de {symbol} após {MaxApiAttempts} tentativas.",
                    ex
                );
            }
        }
    }

    private static PriceZone GetPriceZone(Stock stock, decimal price)
    {
        if (price > stock.SellPrice)
            return PriceZone.Sell;

        if (price < stock.BuyPrice)
            return PriceZone.Buy;

        return PriceZone.Neutral;
    }

    private void HandlePriceZoneChange(Stock stock, StockQuoteResponse response, PriceZone previousZone, PriceZone currentZone)
    {
        if (currentZone == previousZone)
            return;

        if (currentZone == PriceZone.Sell)
            emailService.SendSellAlert(stock, response);

        else if (currentZone == PriceZone.Buy)
            emailService.SendBuyAlert(stock, response);
    }
}
