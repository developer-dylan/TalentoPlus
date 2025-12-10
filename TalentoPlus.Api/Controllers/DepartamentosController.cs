using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Api.DTOs;
using TalentoPlus.Api.Repositories.Interfaces;

namespace TalentoPlus.Api.Controllers;

[Route("api/[controller]")] // api/departamentos
[ApiController]
public class DepartamentosController : ControllerBase
{
    private readonly IDepartmentRepository _deptRepo;

    public DepartamentosController(IDepartmentRepository deptRepo)
    {
        _deptRepo = deptRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var depts = await _deptRepo.GetAllAsync();
        var dtos = depts.Select(d => new DepartmentDto { Id = d.Id, Name = d.Name });
        return Ok(dtos);
    }
}
