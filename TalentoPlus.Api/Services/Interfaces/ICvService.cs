using TalentoPlus.Data.Entities;

namespace TalentoPlus.Api.Services.Interfaces;

public interface ICvService
{
    byte[] GenerateCvPdf(Employee employee);
}
