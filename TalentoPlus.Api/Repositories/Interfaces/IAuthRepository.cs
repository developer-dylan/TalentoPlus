using TalentoPlus.Api.DTOs;

namespace TalentoPlus.Api.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<string?> LoginAsync(LoginDto model);
    Task<bool> RegisterAsync(RegisterDto model);
}
