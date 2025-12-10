using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TalentoPlus.Api.DTOs;
using TalentoPlus.Api.Repositories.Interfaces;
using TalentoPlus.Api.Services.Interfaces;
using TalentoPlus.Data.Entities;
using TalentoPlus.Data.Entities.Enums;

namespace TalentoPlus.Api.Repositories.Implementations;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IEmailService _emailService;

    public AuthRepository(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        IEmployeeRepository employeeRepo,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _employeeRepo = employeeRepo;
        _emailService = emailService;
    }

    public async Task<string?> LoginAsync(LoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (!result.Succeeded)
            return null;

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        
        // Generate JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? "SuperSecretKey1234567890_ChangeMeInProd");
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> RegisterAsync(RegisterDto model)
    {
        // 1. Create Identity User
        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) return false;

        await _userManager.AddToRoleAsync(user, "Employee");

        // 2. Create Employee Record
        var employee = new Employee
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            JobTitle = model.JobTitle,
            DepartmentId = model.DepartmentId,
            Status = EmploymentStatus.Active,
            HireDate = DateTime.UtcNow,
            BirthDate = DateTime.UtcNow.AddYears(-20), // Default
            Address = "Sin dirección",
            Phone = "Sin teléfono",
            ProfessionalProfile = "Nuevo ingreso",
            Education = EducationLevel.Professional,
            Salary = 0
        };

        await _employeeRepo.CreateAsync(employee);

        // 3. Send Welcome Email
        await _emailService.SendWelcomeEmailAsync(model.Email, $"{model.FirstName} {model.LastName}");

        return true;
    }
}
