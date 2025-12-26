using Microsoft.AspNetCore.Mvc;
using EmailServer.Models;
using EmailServer.Services;

namespace EmailServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IEmailService emailService, ILogger<EmailController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.To))
            {
                return BadRequest("At least one recipient is required");
            }

            await _emailService.SendEmailAsync(request);
            return Ok(new { message = "Email sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return StatusCode(500, new { message = "An error occurred while sending the email", error = ex.Message });
        }
    }
}
