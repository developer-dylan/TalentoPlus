using TalentoPlus.Web.DTOs;

namespace TalentoPlus.Web.Repositories.Interfaces
{
    public interface IExcelRepository
    {
        Task SaveEmployeesFromExcelAsync(IEnumerable<ExcelEmployeeDto> employees);
    }
}