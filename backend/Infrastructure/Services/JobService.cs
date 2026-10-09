using Microsoft.EntityFrameworkCore;
using SkillBridge.Application.Exceptions;
using SkillBridge.Application.Interfaces;
using SkillBridge.Domain.Entities;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Infrastructure.Services;

public class JobService : IJobService
{
    private readonly SkillBridgeDbContext _db;

    public JobService(SkillBridgeDbContext db)
    {
        _db = db;
    }

    public async Task<object> GetAllAsync(
        int? skillId = null,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = JobsWithDetails().AsNoTracking();

        if (skillId.HasValue)
        {
            query = query.Where(x =>
                x.RequiredSkills.Any(
                    rs => rs.SkillId == skillId.Value));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();

            query = query.Where(x =>
                x.Title.ToLower().Contains(term) ||
                x.Description.ToLower().Contains(term));
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<object?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await JobsWithDetails()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<object> CreateAsync(
        int userId,
        string title,
        string description,
        List<int> requiredSkillIds,
        CancellationToken cancellationToken = default)
    {
        var employer = await GetEmployerAsync(userId, cancellationToken);

        var skillIds = await ValidateSkillIdsAsync(requiredSkillIds, cancellationToken);

        var job = new Job
        {
            EmployerProfileId = employer.Id,
            Title = title.Trim(),
            Description = description.Trim()
        };

        foreach (var skillId in skillIds)
        {
            job.RequiredSkills.Add(new JobRequiredSkill
            {
                SkillId = skillId
            });
        }

        _db.Jobs.Add(job);

        await _db.SaveChangesAsync(cancellationToken);

             return await LoadJobAsync(job.Id, cancellationToken);
    }

    public async Task<object> UpdateAsync(
        int userId,
        int jobId,
        string title,
        string description,
        List<int> requiredSkillIds,
        CancellationToken cancellationToken = default)
    {
        var employer = await GetEmployerAsync(userId, cancellationToken);

        var job = await _db.Jobs
            .Include(x => x.RequiredSkills)
            .FirstOrDefaultAsync(
                x => x.Id == jobId &&
                     x.EmployerProfileId == employer.Id,
                cancellationToken);

        if (job is null)
            throw new NotFoundException("Job not found.");

        var skillIds = await ValidateSkillIdsAsync(requiredSkillIds, cancellationToken);

        job.Title = title.Trim();
        job.Description = description.Trim();

        // Apply a diff instead of Clear() + re-adding. Re-adding a row with the same
        // composite key (JobId, SkillId) as one being deleted fails in the same SaveChanges.
        var keep = skillIds.ToHashSet();

        var toRemove = job.RequiredSkills
            .Where(x => !keep.Contains(x.SkillId))
            .ToList();

        _db.JobRequiredSkills.RemoveRange(toRemove);

        var existing = job.RequiredSkills
            .Select(x => x.SkillId)
            .ToHashSet();

        foreach (var skillId in skillIds.Where(id => !existing.Contains(id)))
        {
            job.RequiredSkills.Add(new JobRequiredSkill
            {
                JobId = job.Id,
                SkillId = skillId
            });
        }

        await _db.SaveChangesAsync(cancellationToken);

        return await LoadJobAsync(job.Id, cancellationToken);
    }

    public async Task DeleteAsync(
        int userId,
        int jobId,
        CancellationToken cancellationToken = default)
    {
        var employer = await GetEmployerAsync(userId, cancellationToken);

        var job = await _db.Jobs
            .FirstOrDefaultAsync(
                x => x.Id == jobId &&
                     x.EmployerProfileId == employer.Id,
                cancellationToken);

        if (job is null)
            throw new NotFoundException("Job not found.");

        job.IsDeleted = true;

        await _db.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Job> JobsWithDetails() =>
        _db.Jobs
            .Include(x => x.EmployerProfile)
            .Include(x => x.RequiredSkills)
                .ThenInclude(x => x.Skill);

    private Task<Job> LoadJobAsync(int id, CancellationToken cancellationToken) =>
        JobsWithDetails()
            .AsNoTracking()
            .FirstAsync(x => x.Id == id, cancellationToken);

    private async Task<EmployerProfile> GetEmployerAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var employer = await _db.EmployerProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        return employer ?? throw new NotFoundException("Employer profile not found.");
    }

    private async Task<List<int>> ValidateSkillIdsAsync(
        List<int> requiredSkillIds,
        CancellationToken cancellationToken)
    {
        var skillIds = requiredSkillIds.Distinct().ToList();

        if (skillIds.Count == 0)
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["RequiredSkillIds"] = ["At least one required skill is required."]
            });

        var validSkillCount = await _db.Skills
            .CountAsync(x => skillIds.Contains(x.Id), cancellationToken);

        if (validSkillCount != skillIds.Count)
            throw new NotFoundException("One or more required skills were not found.");

        return skillIds;
    }
}
