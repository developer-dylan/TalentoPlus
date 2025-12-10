using TalentoPlus.Data.Entities;

namespace TalentoPlus.Web.Services.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateEmployeePdf(Employee employee); // Employee resume PDF
    }
}