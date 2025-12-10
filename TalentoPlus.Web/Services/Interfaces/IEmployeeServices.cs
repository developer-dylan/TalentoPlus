using TalentoPlus.Data.Entities;

namespace TalentoPlus.Web.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();           // Get all employees
        Task<Employee?> GetByIdAsync(int id);                // Get employee by ID
        Task CreateAsync(Employee employee);                 // Create employee
        Task UpdateAsync(Employee employee);                 // Update employee
        Task DeleteAsync(int id);                            // Delete employee
    }
}