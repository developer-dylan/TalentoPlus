using System.Net;
using System.Net.Mail;
using TalentoPlus.Api.Services.Interfaces;

namespace TalentoPlus.Api.Services.Implementations;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendWelcomeEmailAsync(string email, string name)
    {
        var smtpHost = _configuration["Smtp:Host"];
        var smtpPort = int.Parse(_configuration["Smtp:Port"] ?? "587");
        var smtpUser = _configuration["Smtp:User"];
        var smtpPass = _configuration["Smtp:Password"];

        if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUser))
        {
            Console.WriteLine("SMTP not configured. Skipping email.");
            return;
        }

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUser, "TalentoPlus HR"),
            Subject = "Bienvenido a TalentoPlus",
            Body = $"Hola {name},\n\nBienvenido a TalentoPlus. Tu cuenta ha sido creada exitosamente.\n\nSaludos,\nEquipo de RRHH",
            IsBodyHtml = false
        };

        mailMessage.To.Add(email);

        await client.SendMailAsync(mailMessage);
    }
}
