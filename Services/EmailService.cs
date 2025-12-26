using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using EmailServer.Models;

namespace EmailServer.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfig;

    public EmailService(EmailConfiguration emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public async Task SendEmailAsync(EmailRequest request)
    {
        var email = new MimeMessage();
        
        // From
        email.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromEmail));
        
        // To
        email.To.AddRange(request.To.Split(';').Select(x => new MailboxAddress("", x.Trim())));
        
        // Cc
        if (!string.IsNullOrEmpty(request.Cc))
        {
            email.Cc.AddRange(request.Cc.Split(';').Select(x => new MailboxAddress("", x.Trim())));
        }
        
        // Bcc
        if (!string.IsNullOrEmpty(request.Bcc))
        {
            email.Bcc.AddRange(request.Bcc.Split(';').Select(x => new MailboxAddress("", x.Trim())));
        }
        
        email.Subject = request.Subject;
        
        var builder = new BodyBuilder();
        if (request.IsHtml)
        {
            builder.HtmlBody = request.Body;
        }
        else
        {
            builder.TextBody = request.Body;
        }
        
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        
        // For development, you might want to accept all certificates
        // In production, use proper certificate validation
        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
        
        await smtp.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, 
            _emailConfig.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
        
        if (!string.IsNullOrEmpty(_emailConfig.Username) && !string.IsNullOrEmpty(_emailConfig.Password))
        {
            await smtp.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
        }
        
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
