using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Enums;
using DrinkConnect.Models;
using Microsoft.AspNetCore.Identity;

namespace DrinkConnect.Utils
{
    public static class RoleSeeder
    {
        // Seed roles from UserRole enum
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Enum.GetValues(typeof(UserRole)).Cast<UserRole>())
            {
                var roleName = role.ToString();
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        // Seed default admin user
        public static async Task SeedDefaultAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var adminEmail = "admin@example.com";
            var adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRole.Admin.ToString());
                }
                else
                {
                    throw new Exception("Failed to create default admin");
                }
            }
        }
    }
}
