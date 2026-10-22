using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillBridge.Domain.Enums;

namespace SkillBridge.Infrastructure.Identity;

public static class AdminSeeder
{
    // Program.cs, after `var app = builder.Build();`
    //   using (var scope = app.Services.CreateScope())
    //       await AdminSeeder.SeedAsync(scope.ServiceProvider, app.Configuration);
    public static async Task SeedAsync(IServiceProvider sp, IConfiguration cfg)
    {
        var roles = sp.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var users = sp.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var r in new[] { RoleNames.Candidate, RoleNames.Employer, RoleNames.Admin })
            if (!await roles.RoleExistsAsync(r))
                await roles.CreateAsync(new IdentityRole<int>(r));

        var email = cfg["Admin:Email"];
        var password = cfg["Admin:Password"];   // user-secrets / env var, never commit
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;
        if (await users.FindByEmailAsync(email) is not null) return;

        // If ApplicationUser has other required properties, set them here.
        var admin = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true, IsActive = true };
        var result = await users.CreateAsync(admin, password);
        if (!result.Succeeded)
            throw new InvalidOperationException("Admin seed failed: " + string.Join("; ", result.Errors.Select(e => e.Description)));
        await users.AddToRoleAsync(admin, RoleNames.Admin);
    }
}
