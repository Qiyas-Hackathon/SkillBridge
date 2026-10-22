
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillBridge.Api.Contracts;
using SkillBridge.Domain.Entities;
using SkillBridge.Domain.Enums;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Api.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController(SkillBridgeDbContext db) : ControllerBase
{
    // Assumes Skill has Id and Name, and JobRequiredSkill has JobId, SkillId and a Skill navigation.
    private static IQueryable<JobDto> Project(IQueryable<Job> q) => q.Select(j => new JobDto(
        j.Id, j.EmployerProfileId, j.EmployerProfile.CompanyName, j.Title, j.Description,
        j.RequiredSkills.Select(r => new SkillDto(r.SkillId, r.Skill.Name)).ToList(),
        j.Location, j.JobType, j.SalaryMin, j.SalaryMax, j.SalaryCurrency, j.Deadline, j.IsActive, j.CreatedAt));

    private async Task<int?> MyEmployerId() =>
        await db.EmployerProfiles.Where(p => p.UserId == this.GetUserId()).Select(p => (int?)p.Id).FirstOrDefaultAsync();

    private static void Apply(Job j, JobWriteRequest d)
    {
        j.Title = d.Title.Trim();
        j.Description = d.Description.Trim();
        j.Location = d.Location?.Trim();
        j.JobType = d.JobType;
        j.SalaryMin = d.SalaryMin;
        j.SalaryMax = d.SalaryMax;
        j.SalaryCurrency = d.SalaryCurrency?.ToUpperInvariant();
        j.Deadline = d.Deadline;
    }

    // ---------- Public ----------
    [HttpGet]
    public async Task<PagedResult<JobDto>> List([FromQuery] string? search, [FromQuery] string? location,
        [FromQuery] JobType? type, [FromQuery] decimal? minSalary, [FromQuery] int? skillId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100);
        var today = DateTime.UtcNow.Date;
        var q = db.Jobs.AsNoTracking().Where(j => j.IsActive && (j.Deadline == null || j.Deadline >= today));

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(j => j.Title.Contains(search) || j.Description.Contains(search));
        if (!string.IsNullOrWhiteSpace(location)) q = q.Where(j => j.Location != null && j.Location.Contains(location));
        if (type is not null) q = q.Where(j => j.JobType == type);
        if (minSalary is not null) q = q.Where(j => j.SalaryMax >= minSalary);
        if (skillId is not null) q = q.Where(j => j.RequiredSkills.Any(r => r.SkillId == skillId));

        var total = await q.CountAsync();
        var items = await Project(q.OrderByDescending(j => j.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize)).ToListAsync();
        return new PagedResult<JobDto>(items, total, page, pageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JobDto>> Get(int id)
    {
        var j = await Project(db.Jobs.AsNoTracking().Where(x => x.Id == id && x.IsActive)).FirstOrDefaultAsync();
        return j is null ? NotFound() : j;
    }

    // ---------- Employer ----------
    [HttpGet("mine")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<ActionResult<List<JobDto>>> Mine()
    {
        var eid = await MyEmployerId();
        if (eid is null) return NotFound(new { error = "Employer profile not found." });
        return await Project(db.Jobs.AsNoTracking().Where(j => j.EmployerProfileId == eid).OrderByDescending(j => j.CreatedAt)).ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<ActionResult<JobDto>> Create([FromBody] JobWriteRequest dto)
    {
        var eid = await MyEmployerId();
        if (eid is null) return NotFound(new { error = "Employer profile not found." });

        var skillIds = await ValidSkillIds(dto.SkillIds);
        var job = new Job { EmployerProfileId = eid.Value };
        Apply(job, dto);
        foreach (var sid in skillIds) job.RequiredSkills.Add(new JobRequiredSkill { SkillId = sid });

        db.Jobs.Add(job);
        await db.SaveChangesAsync();
        var result = await Project(db.Jobs.AsNoTracking().Where(j => j.Id == job.Id)).FirstAsync();
        return CreatedAtAction(nameof(Get), new { id = job.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<ActionResult<JobDto>> Update(int id, [FromBody] JobWriteRequest dto)
    {
        var eid = await MyEmployerId();
        var job = await db.Jobs.Include(j => j.RequiredSkills).FirstOrDefaultAsync(j => j.Id == id && j.EmployerProfileId == eid);
        if (job is null) return NotFound();

        Apply(job, dto);
        var wanted = (await ValidSkillIds(dto.SkillIds)).ToHashSet();
        foreach (var r in job.RequiredSkills.Where(r => !wanted.Contains(r.SkillId)).ToList()) job.RequiredSkills.Remove(r);
        foreach (var sid in wanted.Where(s => job.RequiredSkills.All(r => r.SkillId != s)))
            job.RequiredSkills.Add(new JobRequiredSkill { SkillId = sid });

        await db.SaveChangesAsync();
        return await Project(db.Jobs.AsNoTracking().Where(j => j.Id == id)).FirstAsync();
    }

    [HttpPatch("{id:int}/active")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<IActionResult> SetActive(int id, [FromBody] SetActiveRequest dto)
    {
        var eid = await MyEmployerId();
        var job = await db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployerProfileId == eid);
        if (job is null) return NotFound();
        job.IsActive = dto.IsActive;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<IActionResult> Delete(int id)
    {
        var eid = await MyEmployerId();
        var job = await db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployerProfileId == eid);
        if (job is null) return NotFound();
        job.IsDeleted = true; // soft delete, consistent with the rest of the model
        await db.SaveChangesAsync();
        return NoContent();
    }

    private Task<List<int>> ValidSkillIds(IEnumerable<int> ids)
    {
        var distinct = ids.Distinct().ToList();
        return db.Skills.Where(s => distinct.Contains(s.Id)).Select(s => s.Id).ToListAsync();
    }
}
