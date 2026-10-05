using Microsoft.EntityFrameworkCore;
using SkillBridge.Application.Interfaces;
using SkillBridge.Domain.Entities;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Application.Services;

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
            .Include(x => x.Jobs)
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
        var existing = await _db.EmployerProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException(
                "Employer profile already exists.");

        var profile = new EmployerProfile
        {
            UserId = userId,
            CompanyName = companyName.Trim(),
            Description = description?.Trim()
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
            throw new KeyNotFoundException(
                "Employer profile not found.");

        profile.CompanyName = companyName.Trim();
        profile.Description = description?.Trim();

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
            throw new KeyNotFoundException(
                "Employer profile not found.");

        profile.IsDeleted = true;

        await _db.SaveChangesAsync(cancellationToken);
    }
}