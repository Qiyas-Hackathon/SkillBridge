using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SkillBridge.Domain.Constants;

namespace SkillBridge.Infrastructure.Identity;

public static class IdentitySeeder
{

    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

        foreach (var roleName in RoleNames.All)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            var result = await roleManager.CreateAsync(new IdentityRole<int>(roleName));

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    $"Could not create role '{roleName}': " +
                    string.Join("; ", result.Errors.Select(e => e.Description)));
        }
    }
}
