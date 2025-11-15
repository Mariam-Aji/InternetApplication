using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace WebAPI.Infrastructure.Email;

public class SmtpSettings
{
    public string Host { get; set; } = "";
    public int Port { get; set; }
    public bool UseSsl { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string From { get; set; } = "mariamaji442@gmail.com";
}

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _settings;
    public SmtpEmailSender(IOptions<SmtpSettings> options) { _settings = options.Value; }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            EnableSsl = _settings.UseSsl

        };
      


        var msg = new MailMessage(_settings.From, toEmail, subject, htmlBody) { IsBodyHtml = true };
        await client.SendMailAsync(msg);
    }
}
