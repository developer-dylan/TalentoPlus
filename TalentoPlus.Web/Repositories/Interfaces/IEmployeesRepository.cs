using TalentoPlus.Data.Entities;

namespace TalentoPlus.Web.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();       
        Task<Employee?> GetByIdAsync(int id);              
        Task AddAsync(Employee employee);                   
        Task UpdateAsync(Employee employee);                
        Task DeleteAsync(Employee employee);                
        Task<bool> ExistsAsync(int id);                     
        
        Task<IEnumerable<Employee>> GetEmployeesWithDepartmentAsync();
        Task<Employee?> GetEmployeeDetailedAsync(int id);
    }
}