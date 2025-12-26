using Microsoft.AspNetCore.Mvc;
using EmailServer.Services;
using EmailServer.Models;

namespace EmailServer.Controllers;

[ApiController]
[Route("api/test")]
public class TestEmailController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<TestEmailController> _logger;

    public TestEmailController(IEmailService emailService, ILogger<TestEmailController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>
    /// Sends a test email to the specified recipient
    /// </summary>
    /// <param name="recipientEmail">The email address of the recipient</param>
    /// <returns>Status of the email sending operation</returns>
    [HttpPost("send-test-email")]
    public async Task<IActionResult> SendTestEmail([FromQuery] string recipientEmail = "recipient@example.com")
    {
        try
        {
            var emailRequest = new EmailRequest
            {
                To = recipientEmail,
                Subject = "Test Email from Email Server",
                Body = "<h1>Hello from Email Server!</h1>" +
                      "<p>This is a <strong>test email</strong> sent from our email service.</p>" +
                      "<p>Current time: " + DateTime.Now.ToString("F") + "</p>",
                IsHtml = true
            };

            await _emailService.SendEmailAsync(emailRequest);
            
            return Ok(new 
            { 
                success = true, 
                message = "Test email sent successfully!" ,
                details = new { 
                    to = recipientEmail,
                    sentAt = DateTime.UtcNow
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending test email");
            return StatusCode(500, new 
            { 
                success = false, 
                message = "Failed to send test email",
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets information about the email service
    /// </summary>
    /// <returns>Information about the email service</returns>
    [HttpGet("service-info")]
    public IActionResult GetServiceInfo()
    {
        return Ok(new 
        {
            service = "Email Server",
            version = "1.0.0",
            status = "Running",
            timestamp = DateTime.UtcNow,
            endpoints = new[]
            {
                new { method = "POST", path = "/api/email/send", description = "Send an email" },
                new { method = "POST", path = "/api/test/send-test-email", description = "Send a test email" },
                new { method = "GET", path = "/api/test/service-info", description = "Get service information" }
            }
        });
    }
}
