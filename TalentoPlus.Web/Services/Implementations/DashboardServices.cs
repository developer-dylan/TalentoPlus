using Microsoft.EntityFrameworkCore;
using TalentoPlus.Web.Data;
using TalentoPlus.Data;
using TalentoPlus.Data.Entities;
using TalentoPlus.Data.Entities.Enums;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalEmployeesAsync()
        {
            return await _context.Employees.CountAsync();
        }

        public async Task<int> GetEmployeesOnVacationAsync()
        {
            return await _context.Employees
                .CountAsync(e => e.Status == EmploymentStatus.Vacation);
        }

        public async Task<int> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .CountAsync(e => e.Status == EmploymentStatus.Active);
        }
    }
}