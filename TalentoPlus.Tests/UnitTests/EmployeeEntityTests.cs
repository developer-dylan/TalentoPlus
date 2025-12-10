using TalentoPlus.Data.Entities;
using Xunit;

namespace TalentoPlus.Tests.UnitTests;

public class EmployeeEntityTests
{
    [Fact]
    public void FullName_ReturnsConcatenatedName()
    {
        // Arrange
        var emp = new Employee { FirstName = "John", LastName = "Doe" };

        // Act
        var fullName = emp.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }
}
