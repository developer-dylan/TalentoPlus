using System.ComponentModel.DataAnnotations;
using TalentoPlus.Data.Entities.Enums;

namespace TalentoPlus.Web.ViewModels.Employees
{
    public class CreateEmployeeViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public decimal Salary { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required]
        public EmploymentStatus Status { get; set; }

        [Required]
        public EducationLevel Education { get; set; }

        public string ProfessionalProfile { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }
    }
}