# Email Server

A simple .NET Web API for sending emails using SMTP.

## Configuration

1. Update the `appsettings.json` file with your email provider's SMTP settings:
   - `SmtpServer`: Your SMTP server address (e.g., smtp.gmail.com)
   - `Port`: SMTP port (usually 587 for TLS)
   - `Username`: Your email address
   - `Password`: Your email password or app-specific password
   - `FromName`: The display name for the sender

## Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- SMTP server credentials (e.g., Gmail, SendGrid, etc.)

### Configuration

1. Update the `appsettings.json` file with your SMTP server details:

```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "Port": 587,
  "Username": "your-email@gmail.com",
  "Password": "your-app-specific-password",
  "FromName": "Your Application Name",
  "FromEmail": "your-email@gmail.com",
  "UseSsl": true
}
```

> **Note**: For Gmail, you'll need to generate an App Password if you have 2FA enabled.

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Update `appsettings.json` with your SMTP credentials
4. Run the application:
   ```bash
   dotnet run
   ```
5. The API will be available at:
   - `https://localhost:5001` (HTTPS)
   - `http://localhost:5000` (HTTP)
6. Access the Swagger UI at the root URL (`/`) to test the API endpoints

## API Documentation

### Send Email

Send a custom email with full control over all parameters.

```
POST /api/email/send
```

**Request Body:**

```json
{
  "to": "recipient1@example.com;recipient2@example.com",
  "cc": "cc1@example.com;cc2@example.com",
  "bcc": "bcc@example.com",
  "subject": "Your Email Subject",
  "body": "<h1>Hello!</h1><p>This is an <strong>HTML email</strong>.</p>",
  "isHtml": true
}
```

**Response:**

```json
{
  "message": "Email sent successfully"
}
```

### Send Test Email

Quickly test the email service with a pre-formatted HTML email.

```
POST /api/test/send-test-email?recipientEmail=test@example.com
```

**Response:**

```json
{
  "success": true,
  "message": "Test email sent successfully!",
  "details": {
    "to": "test@example.com",
    "sentAt": "2025-12-26T06:10:00Z"
  }
}
```

## Project Structure

```bash
EmailServer/
├── Controllers/
│   ├── EmailController.cs       # Main email API endpoints
│   └── TestEmailController.cs   # Test endpoint for quick verification
├── Models/
│   ├── EmailConfiguration.cs    # SMTP configuration model
│   └── EmailRequest.cs          # Email request DTO
├── Services/
│   ├── IEmailService.cs         # Email service interface
│   └── EmailService.cs          # Email service implementation
├── Program.cs                   # Application entry point
└── appsettings.json             # Configuration file
```

## Security Considerations

### For Production Use

1. **Never commit sensitive data** to version control:
   - Move SMTP credentials to environment variables or Azure Key Vault
   - Use `dotnet user-secrets` for local development:
     ```bash
     dotnet user-secrets init
     dotnet user-secrets set "EmailSettings:Username" "your-email@example.com"
     dotnet user-secrets set "EmailSettings:Password" "your-secure-password"
     ```

2. **Certificate Validation**:
   - The current implementation disables SSL certificate validation for development
   - In production, implement proper certificate validation

3. **Rate Limiting**:
   - Consider implementing rate limiting to prevent abuse
   - Example using `AspNetCoreRateLimit`:
     ```csharp
     services.AddMemoryCache();
     services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
     services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
     services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
     services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
     ```

4. **CORS**:
   - Configure CORS to restrict which domains can access your API
   - Example:
     ```csharp
     services.AddCors(options =>
     {
         options.AddPolicy("AllowSpecificOrigin",
             builder => builder.WithOrigins("https://yourdomain.com")
                              .AllowAnyMethod()
                              .AllowAnyHeader());
     });
     ```

## Troubleshooting

### Common Issues

1. **Authentication Failed**
   - Verify your SMTP credentials
   - For Gmail, ensure you're using an App Password if 2FA is enabled
   - Check if your email provider requires enabling "Less secure app access"

2. **Connection Issues**
   - Verify the SMTP server address and port
   - Check if your network allows outbound SMTP connections
   - Try with `"UseSsl": false` if your server doesn't support SSL/TLS

3. **Email Not Received**
   - Check the spam/junk folder
   - Verify the recipient email address
   - Check the API response for any error messages

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with [.NET 6.0](https://dotnet.microsoft.com/)
- Uses [MailKit](https://github.com/jstedfast/MailKit) for robust email handling
- [Swagger/OpenAPI](https://swagger.io/) for API documentation
