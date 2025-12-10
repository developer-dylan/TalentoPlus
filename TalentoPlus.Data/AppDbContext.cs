using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TalentoPlus.Data.Entities;

namespace TalentoPlus.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // RELATIONSHIP CONFIGURATION
        modelBuilder.Entity<Department>()
            .HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId);
        
        // EMPLOYEE PROPERTY CONFIGURATIONS
        modelBuilder.Entity<Employee>()
            .Property(e => e.FirstName)
            .HasMaxLength(100);

        modelBuilder.Entity<Employee>()
            .Property(e => e.LastName)
            .HasMaxLength(100);

        modelBuilder.Entity<Employee>()
            .Property(e => e.Email)
            .HasMaxLength(150);

        // DEPARTMENT NAME UNIQUE CONSTRAINT
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Name)
            .IsUnique();
    }
}
