namespace EmailServer.Models;

public class EmailConfiguration
{
    public const string SectionName = "EmailSettings";
    
    public required string SmtpServer { get; set; }
    public int Port { get; set; } = 587;
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string FromName { get; set; }
    public required string FromEmail { get; set; }
    public bool UseSsl { get; set; } = true;
}
