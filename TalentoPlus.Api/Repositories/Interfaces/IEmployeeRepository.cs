using TalentoPlus.Data.Entities;

namespace TalentoPlus.Api.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByEmailAsync(string email);
    Task CreateAsync(Employee employee);
}
