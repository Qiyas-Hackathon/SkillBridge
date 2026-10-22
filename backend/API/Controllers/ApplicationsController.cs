using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillBridge.Api.Contracts;
using SkillBridge.Domain.Entities;
using SkillBridge.Domain.Enums;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Api.Controllers;

[ApiController]
[Authorize]
public class ApplicationsController(SkillBridgeDbContext db) : ControllerBase
{
    private IQueryable<ApplicationDto> Project(IQueryable<JobApplication> q) => q.Select(a => new ApplicationDto(
        a.Id, a.JobId, a.Job.Title, a.Job.EmployerProfile.CompanyName,
        a.CandidateProfileId, a.CandidateProfile.FullName,
        db.Users.Where(u => u.Id == a.CandidateProfile.UserId).Select(u => u.Email).FirstOrDefault(),
        a.CandidateProfile.Headline, a.CandidateProfile.CvPath, a.CandidateProfile.GitHubUrl,
        a.CandidateProfile.PortfolioUrl, a.CoverLetter, a.Status, a.AppliedAt, a.UpdatedAt));

    private Task<int?> MyCandidateId() =>
        db.CandidateProfiles.Where(p => p.UserId == this.GetUserId()).Select(p => (int?)p.Id).FirstOrDefaultAsync();

    private Task<int?> MyEmployerId() =>
        db.EmployerProfiles.Where(p => p.UserId == this.GetUserId()).Select(p => (int?)p.Id).FirstOrDefaultAsync();

    // ---------------- Candidate ----------------
    [HttpPost("api/jobs/{jobId:int}/applications")]
    [Authorize(Roles = RoleNames.Candidate)]
    public async Task<IActionResult> Apply(int jobId, [FromBody] ApplyRequest dto)
    {
        var cid = await MyCandidateId();
        if (cid is null) return BadRequest(new { error = "Complete your candidate profile before applying." });

        var job = await db.Jobs.AsNoTracking().FirstOrDefaultAsync(j => j.Id == jobId);
        if (job is null || !job.IsActive) return NotFound(new { error = "Job not found or closed." });
        if (job.Deadline is not null && job.Deadline.Value.Date < DateTime.UtcNow.Date)
            return BadRequest(new { error = "The application deadline has passed." });

        if (await db.JobApplications.AnyAsync(a => a.JobId == jobId && a.CandidateProfileId == cid))
            return Conflict(new { error = "You have already applied to this job." });

        var app = new JobApplication { JobId = jobId, CandidateProfileId = cid.Value, CoverLetter = dto.CoverLetter?.Trim() };
        db.JobApplications.Add(app);
        await db.SaveChangesAsync();

        var result = await Project(db.JobApplications.AsNoTracking().Where(a => a.Id == app.Id)).FirstAsync();
        return CreatedAtAction(nameof(MyApplications), result);
    }

    [HttpGet("api/applications/me")]
    [Authorize(Roles = RoleNames.Candidate)]
    public async Task<ActionResult<List<ApplicationDto>>> MyApplications()
    {
        var cid = await MyCandidateId();
        return await Project(db.JobApplications.AsNoTracking().Where(a => a.CandidateProfileId == cid)
            .OrderByDescending(a => a.AppliedAt)).ToListAsync();
    }

    [HttpDelete("api/applications/{id:int}")]
    [Authorize(Roles = RoleNames.Candidate)]
    public async Task<IActionResult> Withdraw(int id)
    {
        var cid = await MyCandidateId();
        var app = await db.JobApplications.FirstOrDefaultAsync(a => a.Id == id && a.CandidateProfileId == cid);
        if (app is null) return NotFound();
        if (app.Status == ApplicationStatus.Accepted)
            return BadRequest(new { error = "An accepted application cannot be withdrawn." });
        db.JobApplications.Remove(app);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ---------------- Employer ----------------
    [HttpGet("api/jobs/{jobId:int}/applications")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<IActionResult> ForJob(int jobId, [FromQuery] ApplicationStatus? status)
    {
        var eid = await MyEmployerId();
        if (!await db.Jobs.AnyAsync(j => j.Id == jobId && j.EmployerProfileId == eid)) return NotFound();

        var q = db.JobApplications.AsNoTracking().Where(a => a.JobId == jobId);
        if (status is not null) q = q.Where(a => a.Status == status);
        return Ok(await Project(q.OrderByDescending(a => a.AppliedAt)).ToListAsync());
    }

    [HttpGet("api/employer/applications")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<ActionResult<List<ApplicationDto>>> AllForEmployer([FromQuery] ApplicationStatus? status)
    {
        var eid = await MyEmployerId();
        var q = db.JobApplications.AsNoTracking().Where(a => a.Job.EmployerProfileId == eid);
        if (status is not null) q = q.Where(a => a.Status == status);
        return await Project(q.OrderByDescending(a => a.AppliedAt)).ToListAsync();
    }

    [HttpPatch("api/applications/{id:int}/status")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateApplicationStatusRequest dto)
    {
        var eid = await MyEmployerId();
        var app = await db.JobApplications.FirstOrDefaultAsync(a => a.Id == id && a.Job.EmployerProfileId == eid);
        if (app is null) return NotFound();
        app.Status = dto.Status;
        app.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(await Project(db.JobApplications.AsNoTracking().Where(a => a.Id == id)).FirstAsync());
    }
}
