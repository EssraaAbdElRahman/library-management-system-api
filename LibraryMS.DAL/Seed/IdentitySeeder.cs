using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LibraryMS.DAL.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                .CreateLogger("IdentitySeeder");

            await SeedRolesAsync(roleManager, logger);
            await SeedAdminUserAsync(userManager, configuration, logger);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            string[] roles = { "Admin", "Librarian" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (result.Succeeded)
                        logger.LogInformation("Role '{Role}' created successfully.", role);
                    else
                        logger.LogError("Failed to create role '{Role}': {Errors}", role,
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static async Task SeedAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger logger)
        {
            var adminEmail = configuration["SeedAdmin:Email"];
            var adminPassword = configuration["SeedAdmin:Password"];
            var adminFullName = configuration["SeedAdmin:FullName"] ?? "System Admin";

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                logger.LogWarning("Seed admin credentials not configured. Skipping admin seeding.");
                return;
            }

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin is not null)
                return;  

            var admin = ApplicationUser.CreateUser(adminFullName, adminEmail);

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
                logger.LogInformation("Default admin user created successfully.");
            }
            else
            {
                logger.LogError("Failed to create default admin: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
