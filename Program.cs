using EmailServer.Models;
using EmailServer.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Email Settings
var emailConfig = builder.Configuration
    .GetSection(EmailConfiguration.SectionName)
    .Get<EmailConfiguration>();

if (emailConfig == null)
{
    throw new InvalidOperationException("Email configuration is missing. Please check your appsettings.json file.");
}

builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Email Server API", 
        Version = "v1",
        Description = "A simple API for sending emails",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "support@example.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Email Server API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root URL
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
