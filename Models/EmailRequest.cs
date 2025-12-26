namespace EmailServer.Models;

public class EmailRequest
{
    public required string To { get; set; }
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public bool IsHtml { get; set; } = false;
}
