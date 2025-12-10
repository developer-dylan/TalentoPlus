using TalentoPlus.Data.Entities;
using TalentoPlus.Web.Repositories.Interfaces;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _repo.GetEmployeeDetailedAsync(id);
        }

        public async Task CreateAsync(Employee employee)
        {
            await _repo.AddAsync(employee);
        }

        public async Task UpdateAsync(Employee employee)
        {
            await _repo.UpdateAsync(employee);
        }

        public async Task DeleteAsync(int id)
        {
            var emp = await _repo.GetByIdAsync(id);
            if (emp == null)
                return;

            await _repo.DeleteAsync(emp);
        }

    }
}