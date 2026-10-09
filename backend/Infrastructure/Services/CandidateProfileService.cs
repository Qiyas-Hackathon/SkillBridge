using Microsoft.EntityFrameworkCore;
using SkillBridge.Application.Exceptions;
using SkillBridge.Application.Interfaces;
using SkillBridge.Domain.Entities;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Infrastructure.Services;

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
        // IgnoreQueryFilters: a soft-deleted profile still occupies the unique UserId index,
        // so inserting a second row would fail. Revive the old row instead.
        var existing = await _db.CandidateProfiles
            .IgnoreQueryFilters()
            .Include(x => x.CandidateSkills)
                .ThenInclude(x => x.Skill)
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (existing is not null && !existing.IsDeleted)
            throw new ConflictException("Candidate profile already exists.");

        if (existing is not null)
        {
            _db.CandidateSkills.RemoveRange(existing.CandidateSkills);
            existing.CandidateSkills.Clear();

            Apply(existing, fullName, institution, headline, fieldOfStudy,
                degreeLevel, graduationYear, githubUrl, portfolioUrl);
            existing.IsDeleted = false;

            await _db.SaveChangesAsync(cancellationToken);

            return existing;
        }

        var profile = new CandidateProfile { UserId = userId };

        Apply(profile, fullName, institution, headline, fieldOfStudy,
            degreeLevel, graduationYear, githubUrl, portfolioUrl);

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
        // Skills must be loaded: the handler maps profile.CandidateSkills into the response,
        // and without the Include the response always showed an empty skill list.
        var profile = await _db.CandidateProfiles
            .Include(x => x.CandidateSkills)
                .ThenInclude(x => x.Skill)
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (profile is null)
            throw new NotFoundException("Candidate profile not found.");

        Apply(profile, fullName, institution, headline, fieldOfStudy,
            degreeLevel, graduationYear, githubUrl, portfolioUrl);

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
            throw new NotFoundException("Candidate profile not found.");

        var skillExists = await _db.Skills
            .AnyAsync(x => x.Id == skillId, cancellationToken);

        if (!skillExists)
            throw new NotFoundException("Skill not found.");

        var alreadyAdded = await _db.CandidateSkills
            .AnyAsync(
                x => x.CandidateProfileId == profile.Id &&
                     x.SkillId == skillId,
                cancellationToken);

        if (alreadyAdded)
            throw new ConflictException("Skill already added to profile.");

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
            throw new NotFoundException("Candidate profile not found.");

        var candidateSkill = await _db.CandidateSkills
            .FirstOrDefaultAsync(
                x => x.CandidateProfileId == profile.Id &&
                     x.SkillId == skillId,
                cancellationToken);

        if (candidateSkill is null)
            throw new NotFoundException("Skill is not attached to this profile.");

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
            throw new NotFoundException("Candidate profile not found.");

        profile.IsDeleted = true;

        await _db.SaveChangesAsync(cancellationToken);
    }

    private static void Apply(
        CandidateProfile profile,
        string fullName,
        string institution,
        string? headline,
        string? fieldOfStudy,
        string? degreeLevel,
        int? graduationYear,
        string? githubUrl,
        string? portfolioUrl)
    {
        profile.FullName = fullName.Trim();
        profile.Institution = institution.Trim();
        profile.Headline = NullIfBlank(headline);
        profile.FieldOfStudy = NullIfBlank(fieldOfStudy);
        profile.DegreeLevel = NullIfBlank(degreeLevel);
        profile.GraduationYear = graduationYear;
        profile.GitHubUrl = NullIfBlank(githubUrl);
        profile.PortfolioUrl = NullIfBlank(portfolioUrl);
    }

    private static string? NullIfBlank(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
