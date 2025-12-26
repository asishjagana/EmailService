using EmailServer.Models;

namespace EmailServer.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest request);
}
