using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillBridge.Api.Contracts;
using SkillBridge.Domain.Enums;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Api.Controllers;

[ApiController]
[Authorize(Roles = RoleNames.Admin)]
[Route("api/admin")]
public class AdminController(SkillBridgeDbContext db) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<AdminStatsDto> Stats()
    {
        var today = DateTime.UtcNow.Date;
        var byStatus = await db.JobApplications.GroupBy(a => a.Status)
            .Select(g => new { g.Key, Count = g.Count() }).ToListAsync();

        return new AdminStatsDto(
            await db.Users.CountAsync(),
            await db.CandidateProfiles.CountAsync(),
            await db.EmployerProfiles.CountAsync(),
            await db.Jobs.CountAsync(),
            await db.Jobs.CountAsync(j => j.IsActive && (j.Deadline == null || j.Deadline >= today)),
            await db.JobApplications.CountAsync(),
            Enum.GetValues<ApplicationStatus>().ToDictionary(s => s.ToString(),
                s => byStatus.FirstOrDefault(x => x.Key == s)?.Count ?? 0));
    }

    // ---------- Users ----------
    [HttpGet("users")]
    public async Task<PagedResult<AdminUserDto>> Users([FromQuery] string? role, [FromQuery] string? search,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100);

        var q = from u in db.Users
                join ur in db.UserRoles on u.Id equals ur.UserId
                join r in db.Roles on ur.RoleId equals r.Id
                select new
                {
                    u.Id, u.Email, u.IsActive, Role = r.Name,
                    Name = db.CandidateProfiles.Where(c => c.UserId == u.Id).Select(c => c.FullName).FirstOrDefault()
                        ?? db.EmployerProfiles.Where(e => e.UserId == u.Id).Select(e => e.CompanyName).FirstOrDefault()
                };

        if (!string.IsNullOrWhiteSpace(role)) q = q.Where(x => x.Role == role);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(x => (x.Email != null && x.Email.Contains(search)) || (x.Name != null && x.Name.Contains(search)));

        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        var items = rows.Select(x => new AdminUserDto(x.Id, x.Email ?? "", x.Role ?? "", x.Name ?? x.Email ?? "", x.IsActive)).ToList();
        return new PagedResult<AdminUserDto>(items, total, page, pageSize);
    }

    [HttpPatch("users/{id:int}/active")]
    public async Task<IActionResult> SetUserActive(int id, [FromBody] SetActiveRequest dto)
    {
        var guard = await GuardTarget(id);
        if (guard is not null) return guard;
        await db.Users.Where(u => u.Id == id).ExecuteUpdateAsync(s => s.SetProperty(u => u.IsActive, dto.IsActive));
        return NoContent();
    }

    // Soft delete: hides the profile (and, via query filters, their jobs/applications) and disables login.
    [HttpDelete("users/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var guard = await GuardTarget(id);
        if (guard is not null) return guard;
        await db.CandidateProfiles.Where(p => p.UserId == id).ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDeleted, true));
        await db.EmployerProfiles.Where(p => p.UserId == id).ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDeleted, true));
        await db.Users.Where(u => u.Id == id).ExecuteUpdateAsync(s => s.SetProperty(u => u.IsActive, false));
        return NoContent();
    }

    private async Task<IActionResult?> GuardTarget(int id)
    {
        if (id == this.GetUserId()) return BadRequest(new { error = "You cannot do this to your own account." });
        var role = await (from ur in db.UserRoles join r in db.Roles on ur.RoleId equals r.Id
                          where ur.UserId == id select r.Name).FirstOrDefaultAsync();
        if (!await db.Users.AnyAsync(u => u.Id == id)) return NotFound();
        if (role == RoleNames.Admin) return BadRequest(new { error = "Admin accounts cannot be modified here." });
        return null;
    }

    // ---------- Jobs ----------
    [HttpGet("jobs")]
    public async Task<PagedResult<JobDto>> Jobs([FromQuery] string? search, [FromQuery] bool? active,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100);
        var q = db.Jobs.AsNoTracking().AsQueryable();
        if (active is not null) q = q.Where(j => j.IsActive == active);
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(j => j.Title.Contains(search));

        var total = await q.CountAsync();
        var items = await q.OrderByDescending(j => j.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(j => new JobDto(j.Id, j.EmployerProfileId, j.EmployerProfile.CompanyName, j.Title, j.Description,
                j.RequiredSkills.Select(r => new SkillDto(r.SkillId, r.Skill.Name)).ToList(),
                j.Location, j.JobType, j.SalaryMin, j.SalaryMax, j.SalaryCurrency, j.Deadline, j.IsActive, j.CreatedAt))
            .ToListAsync();
        return new PagedResult<JobDto>(items, total, page, pageSize);
    }

    [HttpPatch("jobs/{id:int}/active")]
    public async Task<IActionResult> SetJobActive(int id, [FromBody] SetActiveRequest dto)
    {
        var n = await db.Jobs.Where(j => j.Id == id).ExecuteUpdateAsync(s => s.SetProperty(j => j.IsActive, dto.IsActive));
        return n == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("jobs/{id:int}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var n = await db.Jobs.Where(j => j.Id == id).ExecuteUpdateAsync(s => s.SetProperty(j => j.IsDeleted, true));
        return n == 0 ? NotFound() : NoContent();
    }

    // ---------- Applications (read-only) ----------
    [HttpGet("applications")]
    public async Task<PagedResult<ApplicationDto>> Applications([FromQuery] ApplicationStatus? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 100);
        var q = db.JobApplications.AsNoTracking().AsQueryable();
        if (status is not null) q = q.Where(a => a.Status == status);

        var total = await q.CountAsync();
        var items = await q.OrderByDescending(a => a.AppliedAt).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(a => new ApplicationDto(a.Id, a.JobId, a.Job.Title, a.Job.EmployerProfile.CompanyName,
                a.CandidateProfileId, a.CandidateProfile.FullName,
                db.Users.Where(u => u.Id == a.CandidateProfile.UserId).Select(u => u.Email).FirstOrDefault(),
                a.CandidateProfile.Headline, a.CandidateProfile.CvPath, a.CandidateProfile.GitHubUrl,
                a.CandidateProfile.PortfolioUrl, a.CoverLetter, a.Status, a.AppliedAt, a.UpdatedAt))
            .ToListAsync();
        return new PagedResult<ApplicationDto>(items, total, page, pageSize);
    }
}
