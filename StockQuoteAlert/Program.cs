using Microsoft.Extensions.Configuration;
public class Program
{
    public static void Main(string[] args)
    {

        //--------------------------------IA--------------------------
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>()
        .Build();

        EmailSettings settings = configuration.GetSection("EmailSettings")
        .Get<EmailSettings>()
        ?? throw new Exception("EmailSettings não encontrado.");
        //--------------------------------fim IA--------------------------

        EmailService emailService = new EmailService(settings);

        emailService.SendEmail("Teste","<b>Email appsettings + User Secrets</b>");

        Console.WriteLine("Email enviado com sucesso.");
    }
}