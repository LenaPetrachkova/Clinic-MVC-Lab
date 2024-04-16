using ClinicDomain.Model;
using Microsoft.AspNetCore.Identity;

namespace ClinicInfrastructure
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("Owner") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Owner"));
            }
            if (await roleManager.FindByNameAsync("Doctor") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Doctor"));
            }
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }
    }
}
