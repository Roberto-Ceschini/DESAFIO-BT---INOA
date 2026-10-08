using System.Net;
using System.Net.Mail;

public class EmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        try
        {

            SmtpClient mySmtpClient = new SmtpClient("my.smtp.exampleserver.net", 587);

            // set smtp-client with basicAuthentication
            //mySmtpClient.UseDefaultCredentials = false; //The default value is false

            System.Net.NetworkCredential basicAuthenticationInfo = new ("username", "password");
            mySmtpClient.Credentials = basicAuthenticationInfo;
            mySmtpClient.EnableSsl = true; //A IA sugeriu habilitar SSL por segurança

            // add from,to mailaddresses
            MailAddress from = new MailAddress("test@example.com", "TestFromName");
            MailAddress to = new MailAddress("test2@example.com", "TestToName");
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

            // add ReplyTo
            //MailAddress replyTo = new MailAddress("reply@example.com");
            //myMail.ReplyToList.Add(replyTo);

            // set subject and encoding
            myMail.Subject = "Test message";
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding
            myMail.Body = "<b>Test Mail</b><br>using <b>HTML</b>.";
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            myMail.IsBodyHtml = true;

            mySmtpClient.Send(myMail);
        }

        catch (SmtpException ex)
        {
            throw new ApplicationException
              ("SmtpException has occured: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}