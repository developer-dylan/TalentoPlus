using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentoPlus.Data.Entities;
using TalentoPlus.Data;

namespace TalentoPlus.Web.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            // CREATE ROLES AND DEFAULT ADMIN USER
            string[] roles = { "Admin", "Employee" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // CREATE DEFAULT ADMIN USER
            string adminEmail = "admin@talento.com";
            string adminPassword = "Admin123.";

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(admin, adminPassword);

                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // CREATE DEFAULT DEPARTMENTS
            if (!context.Departments.Any())
            {
                var departamentos = new List<Department>
                {
                    new Department { Name = "Tecnología" },
                    new Department { Name = "Marketing" },
                    new Department { Name = "Ventas" },
                    new Department { Name = "Administración" },
                    new Department { Name = "Recursos Humanos" }
                };

                context.Departments.AddRange(departamentos);
                await context.SaveChangesAsync();
            }
        }
    }
}
