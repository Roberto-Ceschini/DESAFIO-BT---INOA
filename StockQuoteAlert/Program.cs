using Microsoft.Extensions.Configuration;
public class Program
{
    public static async Task Main(string[] args)
    {

        //--------------------------------IA--------------------------
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>()
        .Build();

        EmailSettings emailsettings = configuration.GetSection("EmailSettings")
        .Get<EmailSettings>()
        ?? throw new Exception("EmailSettings não encontrado.");

        ApiSettings apiSettings = configuration.GetSection("ApiSettings")
        .Get<ApiSettings>()
        ?? throw new Exception("ApiSettings não encontrado.");

        HttpClient httpClient = new HttpClient();
        //--------------------------------fim IA--------------------------

        ApiService apiService = new ApiService(httpClient, apiSettings);
        //EmailService emailService = new EmailService(emailsettings);

        string response = await apiService.GetStockQuoteAsync("PETR4");

        //emailService.SendEmail("Teste","<b>Email appsettings + User Secrets</b>");
        //Console.WriteLine("Email enviado com sucesso.");
    }
}