public class Program
{
    public static void Main(string[] args)
    {
        EmailService emailService = new EmailService();

        emailService.SendEmail(
            "destino@gmail.com",
            "Teste",
            "Funcionou!"
        );

        Console.WriteLine("E-mail enviado.");
    }
}