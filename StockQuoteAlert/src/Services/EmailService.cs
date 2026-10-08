using System.Net;
using System.Net.Mail;

public class EmailService
{
    public void SendEmail(string para, string subject, string body)
    {
        try
        {

            SmtpClient smtpClient = new ("smtp.gmail.com", 587);

            // set smtp-client with basicAuthentication
            //smtpClient.UseDefaultCredentials = false; //O valor default já é false

            System.Net.NetworkCredential basicAuthenticationInfo = new ("email", "password");
            smtpClient.Credentials = basicAuthenticationInfo;
            smtpClient.EnableSsl = true; //A IA sugeriu habilitar SSL por segurança

            // add from,to mailaddresses
            MailAddress from = new("robert17ceschini@gmail.com", "TesteOrigem");
            MailAddress to = new("endorsedjam.20221@poli.ufrj.br", "TesteDestino");
            MailMessage mail = new(from, to)
            {
                // add ReplyTo
                //MailAddress replyTo = new MailAddress("reply@example.com");
                //myMail.ReplyToList.Add(replyTo);

                // set subject and encoding
                Subject = "Test message",
                SubjectEncoding = System.Text.Encoding.UTF8,

                // set body-message and encoding
                Body = "<b>Test Mail</b><br>using <b>HTML</b>.",
                BodyEncoding = System.Text.Encoding.UTF8,
                // text or html
                IsBodyHtml = true
            };

            smtpClient.Send(mail);
        }

        catch (SmtpException ex)
        {
            throw new ApplicationException
              ("SmtpException has occured: " + ex.Message);
        }
    }
}
