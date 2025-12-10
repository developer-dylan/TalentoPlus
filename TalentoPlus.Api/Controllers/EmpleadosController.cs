using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Api.DTOs;
using TalentoPlus.Api.Repositories.Interfaces;
using TalentoPlus.Api.Services.Interfaces;

namespace TalentoPlus.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmpleadosController : ControllerBase
{
    private readonly IEmployeeRepository _empRepo;
    private readonly IAuthRepository _authRepo;
    private readonly ICvService _cvService;

    public EmpleadosController(IEmployeeRepository empRepo, IAuthRepository authRepo, ICvService cvService)
    {
        _empRepo = empRepo;
        _authRepo = authRepo;
        _cvService = cvService;
    }

    // POST /api/empleados/autoregistro
    [HttpPost("autoregistro")]
    public async Task<IActionResult> AutoRegister([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _authRepo.RegisterAsync(model);
        if (!success)
            return BadRequest(new { message = "Error al registrar empleado. El correo podría estar en uso." });

        return Ok(new { message = "Registro exitoso. Se ha enviado un correo de bienvenida." });
    }

    // GET /api/empleados/me
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var emp = await _empRepo.GetByEmailAsync(email);
        if (emp == null) return NotFound(new { message = "Empleado no encontrado." });

        var dto = new EmployeeDto
        {
            Id = emp.Id,
            FullName = emp.FullName,
            Email = emp.Email,
            JobTitle = emp.JobTitle,
            Department = emp.Department?.Name ?? "N/A",
            Status = emp.Status.ToString(),
            Phone = emp.Phone,
            Address = emp.Address,
            Salary = emp.Salary
        };

        return Ok(dto);
    }

    // GET /api/empleados/me/cv
    [Authorize]
    [HttpGet("me/cv")]
    public async Task<IActionResult> DownloadCv()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var emp = await _empRepo.GetByEmailAsync(email);
        if (emp == null) return NotFound(new { message = "Empleado no encontrado." });

        var pdfBytes = _cvService.GenerateCvPdf(emp);
        return File(pdfBytes, "application/pdf", $"CV_{emp.FirstName}_{emp.LastName}.pdf");
    }
}
