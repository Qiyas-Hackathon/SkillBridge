using Microsoft.EntityFrameworkCore;
using SkillBridge.Application.Interfaces;
using SkillBridge.Domain.Entities;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Application.Services;

public class CandidateProfileService : ICandidateProfileService
{
    private readonly SkillBridgeDbContext _db;

    public CandidateProfileService(SkillBridgeDbContext db)
    {
        _db = db;
    }

    public async Task<object?> GetMyProfileAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _db.CandidateProfiles
            .Include(x => x.CandidateSkills)
                .ThenInclude(x => x.Skill)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);
    }

    public async Task<object> CreateAsync(
        int userId,
        string fullName,
        string institution,
        string? headline,
        string? fieldOfStudy,
        string? degreeLevel,
        int? graduationYear,
        string? githubUrl,
        string? portfolioUrl,
        CancellationToken cancellationToken = default)
    {
        var existing = await _db.CandidateProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException(
                "Candidate profile already exists.");

        var profile = new CandidateProfile
        {
            UserId = userId,
            FullName = fullName.Trim(),
            Institution = institution.Trim(),
            Headline = headline?.Trim(),
            FieldOfStudy = fieldOfStudy?.Trim(),
            DegreeLevel = degreeLevel?.Trim(),
            GraduationYear = graduationYear,
            GitHubUrl = githubUrl?.Trim(),
            PortfolioUrl = portfolioUrl?.Trim()
        };

        _db.CandidateProfiles.Add(profile);

        await _db.SaveChangesAsync(cancellationToken);

        return profile;
    }

    public async Task<object> UpdateAsync(
        int userId,
        string fullName,
        string institution,
        string? headline,
        string? fieldOfStudy,
        string? degreeLevel,
        int? graduationYear,
        string? githubUrl,
        string? portfolioUrl,
        CancellationToken cancellationToken = default)
    {
        var profile = await _db.CandidateProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (profile is null)
            throw new KeyNotFoundException(
                "Candidate profile not found.");

        profile.FullName = fullName.Trim();
        profile.Institution = institution.Trim();
        profile.Headline = headline?.Trim();
        profile.FieldOfStudy = fieldOfStudy?.Trim();
        profile.DegreeLevel = degreeLevel?.Trim();
        profile.GraduationYear = graduationYear;
        profile.GitHubUrl = githubUrl?.Trim();
        profile.PortfolioUrl = portfolioUrl?.Trim();

        await _db.SaveChangesAsync(cancellationToken);

        return profile;
    }

    public async Task AddSkillAsync(
        int userId,
        int skillId,
        CancellationToken cancellationToken = default)
    {
        var profile = await _db.CandidateProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (profile is null)
            throw new KeyNotFoundException(
                "Candidate profile not found.");

        var skillExists = await _db.Skills
            .AnyAsync(x => x.Id == skillId, cancellationToken);

        if (!skillExists)
            throw new KeyNotFoundException("Skill not found.");

        var alreadyAdded = await _db.CandidateSkills
            .AnyAsync(
                x => x.CandidateProfileId == profile.Id &&
                     x.SkillId == skillId,
                cancellationToken);

        if (alreadyAdded)
            throw new InvalidOperationException(
                "Skill already added to profile.");

        _db.CandidateSkills.Add(new CandidateSkill
        {
            CandidateProfileId = profile.Id,
            SkillId = skillId
        });

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveSkillAsync(
        int userId,
        int skillId,
        CancellationToken cancellationToken = default)
    {
        var profile = await _db.CandidateProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (profile is null)
            throw new KeyNotFoundException(
                "Candidate profile not found.");

        var candidateSkill = await _db.CandidateSkills
            .FirstOrDefaultAsync(
                x => x.CandidateProfileId == profile.Id &&
                     x.SkillId == skillId,
                cancellationToken);

        if (candidateSkill is null)
            throw new KeyNotFoundException(
                "Skill is not attached to this profile.");

        _db.CandidateSkills.Remove(candidateSkill);

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var profile = await _db.CandidateProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (profile is null)
            throw new KeyNotFoundException(
                "Candidate profile not found.");

        profile.IsDeleted = true;

        await _db.SaveChangesAsync(cancellationToken);
    }
}