using Microsoft.Extensions.Configuration;
using System.Globalization;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            HandleUserExpectedErrors(args);

            string symbol = args[0].ToUpper();
            decimal sellPrice = decimal.Parse(args[1].Replace(',', '.'), CultureInfo.InvariantCulture);
            decimal buyPrice = decimal.Parse(args[2].Replace(',', '.'), CultureInfo.InvariantCulture);

            Stock stock = new(symbol, sellPrice, buyPrice);

            //-------Codigo gerado por IA para configuracao------------------
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

            EmailSettings emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>()
            ?? throw new Exception("EmailSettings não encontrado.");

            ApiSettings apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>()
            ?? throw new Exception("ApiSettings não encontrado.");
            //----------Fim codigo IA -----------------------------------

            HttpClient httpClient = new();

            ApiService apiService = new (httpClient, apiSettings);
            EmailService emailService = new (emailSettings);

            MonitoringService monitoringService = new(apiService, emailService);

            Console.WriteLine($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] Monitorando {stock.Symbol}...");

            await monitoringService.MonitorStockQuoteAsync(stock);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] {ex.Message}");
        }
    }

    private static void HandleUserExpectedErrors(string[] args)
    {
        if (args.Length != 3)
        {
            throw new ArgumentException("Uso correto exemplo: stock-quote-alert PETR4 22.67 22.59");
        }

        //As linhas abaixo foram geradas por IA pois eu nao sabia da funcao TryParse do C#
        if (!decimal.TryParse(args[1].Replace(',', '.'), CultureInfo.InvariantCulture, out _)
            || !decimal.TryParse(args[2].Replace(',', '.'), CultureInfo.InvariantCulture, out _))
        {
            throw new ArgumentException(
                "Os preços informados são inválidos."
            );
        }
    }
}
