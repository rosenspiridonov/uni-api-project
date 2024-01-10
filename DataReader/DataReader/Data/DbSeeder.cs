using Microsoft.AspNetCore.Identity;

using static DataReader.Constants;

namespace DataReader.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager)
        {
            var adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@gmail.com"
            };

            if (userManager.Users.All(u => u.UserName != adminUser.UserName))
            {
                await userManager.CreateAsync(adminUser, "admin");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
