namespace TalentoPlus.Web.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<int> GetTotalEmployeesAsync();
        Task<int> GetEmployeesOnVacationAsync();
        Task<int> GetActiveEmployeesAsync();
    }
}