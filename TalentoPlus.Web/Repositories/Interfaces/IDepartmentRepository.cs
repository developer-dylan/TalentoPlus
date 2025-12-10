using TalentoPlus.Data.Entities;

namespace TalentoPlus.Web.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();        // Get all departments

    }
}