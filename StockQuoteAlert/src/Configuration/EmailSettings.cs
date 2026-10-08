
public class EmailSettings
{
    public string To { get; set; } = "";
    public string From { get; set; } = "";
    public string Password { get; set; } = "";
    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; }
    public bool EnableSsl { get; set; }
}
