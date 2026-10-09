using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillBridge.Application.Commands.Employer;
using SkillBridge.Application.DTOs.Employer;
using SkillBridge.Application.Exceptions;
using SkillBridge.Application.Interfaces;
using SkillBridge.Application.Queries.Employer;
using SkillBridge.Domain.Constants;

namespace SkillBridge.Api.Controllers;

[ApiController]
[Route("api/employers")]
[Authorize(Roles = RoleNames.Employer)]
public class EmployerController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ICurrentUserService _currentUser;

    public EmployerController(ISender sender, ICurrentUserService currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    private int UserId =>
        _currentUser.UserId ?? throw new AuthenticationFailedException("You are not signed in.");

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetMyEmployerProfileQuery(UserId),
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
                UserId,
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
                UserId,
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
            new DeleteEmployerProfileCommand(UserId),
            cancellationToken);

        return NoContent();
    }
}
