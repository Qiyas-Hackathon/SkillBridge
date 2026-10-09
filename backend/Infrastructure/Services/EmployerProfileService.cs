using Microsoft.EntityFrameworkCore;
using SkillBridge.Application.Exceptions;
using SkillBridge.Application.Interfaces;
using SkillBridge.Domain.Entities;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Infrastructure.Services;

public class EmployerProfileService : IEmployerProfileService
{
    private readonly SkillBridgeDbContext _db;

    public EmployerProfileService(SkillBridgeDbContext db)
    {
        _db = db;
    }

    public async Task<object?> GetMyProfileAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _db.EmployerProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);
    }

    public async Task<object> CreateAsync(
        int userId,
        string companyName,
        string? description,
        CancellationToken cancellationToken = default)
    {
        // IgnoreQueryFilters: a soft-deleted profile still occupies the unique UserId index.
        var existing = await _db.EmployerProfiles
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (existing is not null && !existing.IsDeleted)
            throw new ConflictException("Employer profile already exists.");

        if (existing is not null)
        {
            existing.CompanyName = companyName.Trim();
            existing.Description = NullIfBlank(description);
            existing.IsDeleted = false;

            await _db.SaveChangesAsync(cancellationToken);

            return existing;
        }

        var profile = new EmployerProfile
        {
            UserId = userId,
            CompanyName = companyName.Trim(),
            Description = NullIfBlank(description)
        };

        _db.EmployerProfiles.Add(profile);

        await _db.SaveChangesAsync(cancellationToken);

        return profile;
    }

    public async Task<object> UpdateAsync(
        int userId,
        string companyName,
        string? description,
        CancellationToken cancellationToken = default)
    {
        var profile = await _db.EmployerProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (profile is null)
            throw new NotFoundException("Employer profile not found.");

        profile.CompanyName = companyName.Trim();
        profile.Description = NullIfBlank(description);

        await _db.SaveChangesAsync(cancellationToken);

        return profile;
    }

    public async Task DeleteAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var profile = await _db.EmployerProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (profile is null)
            throw new NotFoundException("Employer profile not found.");

        // Jobs of a deleted employer are hidden by the Job query filter.
        profile.IsDeleted = true;

        await _db.SaveChangesAsync(cancellationToken);
    }

    private static string? NullIfBlank(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
