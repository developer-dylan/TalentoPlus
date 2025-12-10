using Microsoft.EntityFrameworkCore;
using TalentoPlus.Web.Data;
using TalentoPlus.Data;
using TalentoPlus.Web.DTOs;
using TalentoPlus.Data.Entities;
using TalentoPlus.Data.Entities.Enums;
using TalentoPlus.Web.Repositories.Interfaces;

namespace TalentoPlus.Web.Repositories.Implementations
{
    public class ExcelRepository : IExcelRepository
    {
        private readonly AppDbContext _context;

        public ExcelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveEmployeesFromExcelAsync(IEnumerable<ExcelEmployeeDto> excelEmployees)
        {
            foreach (var dto in excelEmployees)
            {
                // === Normalize Department ===
                var departmentName = !string.IsNullOrWhiteSpace(dto.Department) ? dto.Department.Trim() : "Sin Departamento";

                var dept = await _context.Departments
                    .FirstOrDefaultAsync(d => d.Name == departmentName);

                if (dept == null)
                {
                    dept = new Department { Name = departmentName };
                    await _context.Departments.AddAsync(dept);
                    await _context.SaveChangesAsync();
                }

                // === Convert dates to UTC to avoid PostgreSQL errors ===
                var birthDate = EnsureUtc(dto.BirthDate);
                var hireDate = EnsureUtc(dto.HireDate);

                // === Create employee entity ===
                var employee = new Employee
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,

                    BirthDate = birthDate,
                    HireDate = hireDate,

                    Address = dto.Address,
                    Phone = dto.Phone,
                    Email = dto.Email,
                    JobTitle = dto.JobTitle,
                    Salary = dto.Salary,

                    Status = NormalizeStatus(dto.Status),
                    Education = NormalizeEducation(dto.Education),

                    ProfessionalProfile = dto.ProfessionalProfile,
                    DepartmentId = dept.Id
                };

                await _context.Employees.AddAsync(employee);
            }

            await _context.SaveChangesAsync();
        }

        // =======================================================
        // Helper: Convert all dates to UTC for PostgreSQL
        // =======================================================
        private DateTime EnsureUtc(DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
                return value;

            return DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        // =======================================================
        // ENUM NORMALIZATION HELPERS
        // =======================================================
        private EmploymentStatus NormalizeStatus(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return EmploymentStatus.Active;

            value = value.Trim().ToLower();

            return value switch
            {
                "activo" or "active" => EmploymentStatus.Active,
                "vacaciones" or "vacation" or "vacations" => EmploymentStatus.Vacation,
                "inactivo" or "inactive" => EmploymentStatus.Inactive,
                _ => EmploymentStatus.Active
            };
        }

        private EducationLevel NormalizeEducation(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return EducationLevel.Professional;

            value = value.Trim().ToLower();

            return value switch
            {
                "bachiller" => EducationLevel.HighSchool,
                "técnico" or "tecnico" => EducationLevel.Technical,
                "tecnólogo" or "tecnologo" => EducationLevel.Technologist,
                "profesional" => EducationLevel.Professional,
                "maestría" or "maestria" => EducationLevel.Master,
                "doctorado" => EducationLevel.Doctorate,
                _ => EducationLevel.Professional
            };
        }
    }
}
