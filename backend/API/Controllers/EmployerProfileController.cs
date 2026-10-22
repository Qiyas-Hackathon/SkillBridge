using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillBridge.Api.Contracts;
using SkillBridge.Domain.Entities;
using SkillBridge.Domain.Enums;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Api.Controllers;

[ApiController]
[Authorize(Roles = RoleNames.Employer)]
[Route("api/employer/profile")]
public class EmployerProfileController(SkillBridgeDbContext db) : ControllerBase
{
    private static EmployerProfileDto ToDto(EmployerProfile p) =>
        new(p.Id, p.CompanyName, p.Description, p.ContactName, p.Website, p.LogoPath);

    [HttpGet]
    public async Task<ActionResult<EmployerProfileDto>> Get()
    {
        var p = await db.EmployerProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == this.GetUserId());
        return p is null ? NotFound() : ToDto(p);
    }

    [HttpPut]
    public async Task<ActionResult<EmployerProfileDto>> Put([FromBody] EmployerProfileRequest dto)
    {
        var p = await db.EmployerProfiles.FirstOrDefaultAsync(x => x.UserId == this.GetUserId());
        if (p is null) return NotFound();
        p.CompanyName = dto.CompanyName.Trim();
        p.Description = dto.Description?.Trim();
        p.ContactName = dto.ContactName?.Trim();
        p.Website = dto.Website?.Trim();
        await db.SaveChangesAsync();
        return ToDto(p);
    }
}
