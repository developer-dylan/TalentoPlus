using TalentoPlus.Data.Entities;

namespace TalentoPlus.Api.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
}
