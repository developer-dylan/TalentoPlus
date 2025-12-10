using TalentoPlus.Api.Services.Implementations;
using TalentoPlus.Data.Entities;
using TalentoPlus.Data.Entities.Enums;
using Xunit;

namespace TalentoPlus.Tests.UnitTests;

public class CvServiceTests
{
    [Fact]
    public void GenerateCvPdf_ReturnsBytes_WhenEmployeeIsValid()
    {
        // Arrange
        var service = new CvService();
        var employee = new Employee
        {
            FirstName = "Juan",
            LastName = "Perez",
            Email = "juan@test.com",
            JobTitle = "Developer",
            Department = new Department { Name = "IT" },
            Status = EmploymentStatus.Active,
            HireDate = DateTime.Now,
            BirthDate = DateTime.Now.AddYears(-30),
            Address = "Calle 123",
            Phone = "555-1234",
            ProfessionalProfile = "Test Profile",
            Education = EducationLevel.Professional,
            Salary = 5000
        };

        // Act
        var result = service.GenerateCvPdf(employee);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);
    }
}
