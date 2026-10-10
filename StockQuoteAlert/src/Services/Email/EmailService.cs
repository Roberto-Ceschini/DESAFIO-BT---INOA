using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly EmailSettings settings;

    public EmailService(EmailSettings settings)
    {
        this.settings = settings;
    }
    public void SendEmail(string subject, string body)
    {
        try
        {

            SmtpClient smtpClient = new(settings.SmtpServer, settings.SmtpPort);

            NetworkCredential basicAuthenticationInfo = new(settings.From, settings.Password);
            smtpClient.Credentials = basicAuthenticationInfo;
            smtpClient.EnableSsl = settings.EnableSsl; //Esta linha foi sugestão de IA para segurança

            // add from,to mailaddresses
            MailAddress from = new(settings.From, "Desafio BT - INOA");
            MailAddress to = new(settings.To);
            MailMessage mail = new(from, to)
            {
                // add ReplyTo
                //MailAddress replyTo = new MailAddress("reply@example.com");
                //myMail.ReplyToList.Add(replyTo);

                // set subject and encoding
                Subject = subject,
                SubjectEncoding = System.Text.Encoding.UTF8,

                // set body-message and encoding
                Body = body,
                BodyEncoding = System.Text.Encoding.UTF8,
                // text or html
                IsBodyHtml = true
            };

            smtpClient.Send(mail);
        }

        catch (SmtpException ex)
        {
            throw new ApplicationException("SmtpException has occured: " + ex.Message);
        }
    }

    public void SendBuyAlert(Stock stock, StockQuoteResponse response)
    {
        string subject = $"Alerta de compra - {stock.Symbol}";

        string body = EmailTemplate.BuildAlert(
            "Oportunidade de compra",
            "COMPRA",
            "#1FA86B",
            stock,
            response.RegularMarketPrice,
            stock.BuyPrice,
            response.RegularMarketTime
        );//Esta linha foi gerada com IA, como inspiração de paleta, o site https://inoa.com/

        SendEmail(subject, body);
    }

    public void SendSellAlert(Stock stock, StockQuoteResponse response)
    {
         string subject = $"Alerta de venda - {stock.Symbol}";

        string body = EmailTemplate.BuildAlert(
            "Alerta de venda",
            "VENDA",
            "#D9534F",
            stock,
            response.RegularMarketPrice,
            stock.SellPrice,
            response.RegularMarketTime
        );//Esta linha foi gerada com IA, como inspiração de paleta, o site https://inoa.com/

        SendEmail(subject, body);
    }
}
