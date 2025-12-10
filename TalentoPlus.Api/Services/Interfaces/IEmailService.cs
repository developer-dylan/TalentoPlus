namespace TalentoPlus.Api.Services.Interfaces;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string name);
}
