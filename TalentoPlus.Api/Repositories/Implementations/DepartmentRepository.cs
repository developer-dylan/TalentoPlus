using Microsoft.EntityFrameworkCore;
using TalentoPlus.Api.Repositories.Interfaces;
using TalentoPlus.Data;
using TalentoPlus.Data.Entities;

namespace TalentoPlus.Api.Repositories.Implementations;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments.ToListAsync();
    }
}
