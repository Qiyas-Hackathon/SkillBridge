using Microsoft.AspNetCore.Identity;
using SkillBridge.Application.Commands.Auth;
using SkillBridge.Application.DTOs.Auth;
using SkillBridge.Application.Exceptions;
using SkillBridge.Application.Interfaces;
using SkillBridge.Domain.Entities;
using SkillBridge.Domain.Enums;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Infrastructure.Identity;

public sealed class AuthService(UserManager<ApplicationUser> userManager, SkillBridgeDbContext db) : IAuthService
{
    public async Task<UserDto> RegisterAsync(RegisterCommand command, CancellationToken ct = default)
    {
        var roleName = command.Role.ToString();

     
        await using var transaction = await db.Database.BeginTransactionAsync(ct);

        var user = new ApplicationUser
        {
            UserName = command.Email,
            Email = command.Email,
            CreatedAt = DateTime.UtcNow
        };

        var created = await userManager.CreateAsync(user, command.Password);
        if (!created.Succeeded)
            throw MapRegistrationFailure(created);

        var roleResult = await userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
            throw new InvalidOperationException(
                $"Could not assign role '{roleName}': " +
                string.Join("; ", roleResult.Errors.Select(e => e.Description)));

       
        switch (command.Role)
        {
            case UserRole.Candidate when command.Candidate is { } c:
                db.CandidateProfiles.Add(new CandidateProfile
                {
                    UserId = user.Id,
                    FullName = c.FullName.Trim(),
                    Headline = NullIfBlank(c.Headline),
                    Institution = c.Institution.Trim(),
                    FieldOfStudy = c.FieldOfStudy.Trim(),
                    EducationLevel = c.DegreeLevel.Trim(),
                    GraduationYear = c.GraduationYear,
                    GitHubUrl = NullIfBlank(c.GitHubUrl),
                    PortfolioUrl = NullIfBlank(c.PortfolioUrl)
                });
                break;

            case UserRole.Employer when command.Employer is { } e:
                db.EmployerProfiles.Add(new EmployerProfile
                {
                    UserId = user.Id,
                    CompanyName = e.CompanyName.Trim(),
                   
                  
                });
                break;

            default:
                
                throw new InvalidOperationException($"Profile details missing for role '{roleName}'.");
        }

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return new UserDto(user.Id, user.Email!, roleName);
    }

    public async Task<UserDto> ValidateCredentialsAsync(
        string email, string password, CancellationToken ct = default)
    {
        const string invalid = "Invalid email or password.";

        var user = await userManager.FindByEmailAsync(email)
                   ?? throw new AuthenticationFailedException(invalid);

        if (await userManager.IsLockedOutAsync(user))
            throw new AuthenticationFailedException("Account is temporarily locked. Try again later.");

        if (!await userManager.CheckPasswordAsync(user, password))
        {
            // Counts toward lockout (5 failures => 5 minute lock, see DependencyInjection).
            await userManager.AccessFailedAsync(user);
            throw new AuthenticationFailedException(invalid);
        }

        await userManager.ResetAccessFailedCountAsync(user);

        var roles = await userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault()
                   ?? throw new ForbiddenException("This account has no role assigned.");

        return new UserDto(user.Id, user.Email!, role);
    }

    public async Task<UserDto?> GetByIdAsync(int userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return null;

        var roles = await userManager.GetRolesAsync(user);
        return new UserDto(user.Id, user.Email!, roles.FirstOrDefault() ?? string.Empty);
    }

    private static string? NullIfBlank(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static Exception MapRegistrationFailure(IdentityResult result)
    {
        if (result.Errors.Any(e => e.Code is "DuplicateUserName" or "DuplicateEmail"))
            return new ConflictException("An account with this email already exists.");

           var errors = result.Errors
            .GroupBy(e => e.Code.StartsWith("Password", StringComparison.Ordinal) ? "Password" : "Email")
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

        return new RequestValidationException(errors);
    }
}
