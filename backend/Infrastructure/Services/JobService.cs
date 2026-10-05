using Microsoft.EntityFrameworkCore;
using SkillBridge.Application.Interfaces;
using SkillBridge.Domain.Entities;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Application.Services;

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
        var query = _db.Jobs
            .Include(x => x.EmployerProfile)
            .Include(x => x.RequiredSkills)
                .ThenInclude(x => x.Skill)
            .AsNoTracking()
            .AsQueryable();

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
        return await _db.Jobs
            .Include(x => x.EmployerProfile)
            .Include(x => x.RequiredSkills)
                .ThenInclude(x => x.Skill)
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
        var employer = await _db.EmployerProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (employer is null)
            throw new KeyNotFoundException(
                "Employer profile not found.");

        if (requiredSkillIds.Count == 0)
            throw new InvalidOperationException(
                "At least one required skill is required.");

        var skillIds = requiredSkillIds.Distinct().ToList();

        var validSkillIds = await _db.Skills
            .Where(x => skillIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (validSkillIds.Count != skillIds.Count)
            throw new KeyNotFoundException(
                "One or more required skills were not found.");

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

        return job;
    }

    public async Task<object> UpdateAsync(
        int userId,
        int jobId,
        string title,
        string description,
        List<int> requiredSkillIds,
        CancellationToken cancellationToken = default)
    {
        var employer = await _db.EmployerProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (employer is null)
            throw new KeyNotFoundException(
                "Employer profile not found.");

        var job = await _db.Jobs
            .Include(x => x.RequiredSkills)
            .FirstOrDefaultAsync(
                x => x.Id == jobId &&
                     x.EmployerProfileId == employer.Id,
                cancellationToken);

        if (job is null)
            throw new KeyNotFoundException(
                "Job not found.");

        var skillIds = requiredSkillIds
            .Distinct()
            .ToList();

        if (skillIds.Count == 0)
            throw new InvalidOperationException(
                "At least one required skill is required.");

        var validSkillIds = await _db.Skills
            .Where(x => skillIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (validSkillIds.Count != skillIds.Count)
            throw new KeyNotFoundException(
                "One or more required skills were not found.");

        job.Title = title.Trim();
        job.Description = description.Trim();

        job.RequiredSkills.Clear();

        foreach (var skillId in skillIds)
        {
            job.RequiredSkills.Add(new JobRequiredSkill
            {
                JobId = job.Id,
                SkillId = skillId
            });
        }

        await _db.SaveChangesAsync(cancellationToken);

        return job;
    }

    public async Task DeleteAsync(
        int userId,
        int jobId,
        CancellationToken cancellationToken = default)
    {
        var employer = await _db.EmployerProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);

        if (employer is null)
            throw new KeyNotFoundException(
                "Employer profile not found.");

        var job = await _db.Jobs
            .FirstOrDefaultAsync(
                x => x.Id == jobId &&
                     x.EmployerProfileId == employer.Id,
                cancellationToken);

        if (job is null)
            throw new KeyNotFoundException(
                "Job not found.");

        job.IsDeleted = true;

        await _db.SaveChangesAsync(cancellationToken);
    }
}