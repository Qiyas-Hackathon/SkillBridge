using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillBridge.Application.Commands.Employer;
using SkillBridge.Application.DTOs.Employer;
using SkillBridge.Application.Queries.Employer;

namespace SkillBridge.Api.Controllers;

[ApiController]
[Route("api/employers")]
[Authorize(Roles = "Employer")]
public class EmployerController : ControllerBase
{
    private readonly ISender _sender;

    public EmployerController(ISender sender)
    {
        _sender = sender;
    }

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
            new GetMyEmployerProfileQuery(GetUserId()),
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost("me")]
    public async Task<IActionResult> Create(
        CreateEmployerProfileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new CreateEmployerProfileCommand(
                GetUserId(),
                request.CompanyName,
                request.Description),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetMe),
            null,
            result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> Update(
        UpdateEmployerProfileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new UpdateEmployerProfileCommand(
                GetUserId(),
                request.CompanyName,
                request.Description),
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("me")]
    public async Task<IActionResult> Delete(
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteEmployerProfileCommand(GetUserId()),
            cancellationToken);

        return NoContent();
    }
}