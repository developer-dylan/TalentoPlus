using Microsoft.EntityFrameworkCore;
using TalentoPlus.Web.Data;
using TalentoPlus.Data;
using TalentoPlus.Data.Entities;
using TalentoPlus.Web.Repositories.Interfaces;

namespace TalentoPlus.Web.Repositories.Implementations
{
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

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.Name == name);
        }

        public async Task AddAsync(Department dept)
        {
            await _context.Departments.AddAsync(dept);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department dept)
        {
            _context.Departments.Update(dept);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department dept)
        {
            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
        }
    }
}