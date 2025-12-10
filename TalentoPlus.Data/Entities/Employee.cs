using System.ComponentModel.DataAnnotations.Schema;
using TalentoPlus.Data.Entities.Enums;

namespace TalentoPlus.Data.Entities;

public class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }

    public EmploymentStatus Status { get; set; }
    public EducationLevel Education { get; set; }

    public string ProfessionalProfile { get; set; } = null!;

    // Relationship with Department
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    // Computed field (NOT mapped to DB)
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
