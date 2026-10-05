using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillBridge.Application.Commands.Skill;
using SkillBridge.Application.DTOs.Skills;
using SkillBridge.Application.Queries.Skill;

namespace SkillBridge.Api.Controllers;

[ApiController]
[Route("api/skills")]
public class SkillController : ControllerBase
{
    private readonly ISender _sender;

    public SkillController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetSkillsQuery(),
            cancellationToken);

        return Ok(result);
    }

    // Temporary/simple endpoint for creating skills.
    // We can restrict this to Admin after teammate's auth is finalized.
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateSkillRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new CreateSkillCommand(request.Name),
            cancellationToken);

        return Ok(result);
    }
}