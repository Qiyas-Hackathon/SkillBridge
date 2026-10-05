using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillBridge.Application.Commands.Candidate;
using SkillBridge.Application.DTOs.Candidate;
using SkillBridge.Application.Queries.Candidate;

namespace SkillBridge.Api.Controllers;

[ApiController]
[Route("api/candidates")]
[Authorize(Roles = "Candidate")]
public class CandidateController : ControllerBase
{
    private readonly ISender _sender;

    public CandidateController(ISender sender)
    {
        _sender = sender;
    }

    // TEMPORARY: replace with your teammate's ICurrentUserService once we wire it.
    private int GetUserId()
    {
        return int.Parse(
            User.FindFirst("sub")?.Value
            ?? User.FindFirst("userId")?.Value
            ?? throw new UnauthorizedAccessException());
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetMyCandidateProfileQuery(GetUserId()),
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost("me")]
    public async Task<IActionResult> Create(
        CreateCandidateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new CreateCandidateProfileCommand(
                GetUserId(),
                request.FullName,
                request.Institution,
                request.Headline,
                request.FieldOfStudy,
                request.DegreeLevel,
                request.GraduationYear,
                request.GitHubUrl,
                request.PortfolioUrl),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetMe),
            result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> Update(
        UpdateCandidateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new UpdateCandidateProfileCommand(
                GetUserId(),
                request.FullName,
                request.Institution,
                request.Headline,
                request.FieldOfStudy,
                request.DegreeLevel,
                request.GraduationYear,
                request.GitHubUrl,
                request.PortfolioUrl),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("me/skills/{skillId:int}")]
    public async Task<IActionResult> AddSkill(
        int skillId,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new AddCandidateSkillCommand(GetUserId(), skillId),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("me/skills/{skillId:int}")]
    public async Task<IActionResult> RemoveSkill(
        int skillId,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new RemoveCandidateSkillCommand(GetUserId(), skillId),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("me")]
    public async Task<IActionResult> Delete(
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteCandidateProfileCommand(GetUserId()),
            cancellationToken);

        return NoContent();
    }
}