using TalentoPlus.Web.DTOs;

namespace TalentoPlus.Web.Services.Interfaces
{
    public interface IExcelService
    {
        Task<bool> ProcessExcelAsync(IFormFile file);
    }
}