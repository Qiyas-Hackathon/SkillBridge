using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillBridge.Application.Commands.Job;
using SkillBridge.Application.DTOs.Jobs;
using SkillBridge.Application.Exceptions;
using SkillBridge.Application.Interfaces;
using SkillBridge.Application.Queries.Job;
using SkillBridge.Domain.Constants;

namespace SkillBridge.Api.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ICurrentUserService _currentUser;

    public JobController(ISender sender, ICurrentUserService currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    private int UserId =>
        _currentUser.UserId ?? throw new AuthenticationFailedException("You are not signed in.");

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? skillId,
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetJobsQuery(skillId, search),
            cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetJobByIdQuery(id),
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [Authorize(Roles = RoleNames.Employer)]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateJobRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new CreateJobCommand(
                UserId,
                request.Title,
                request.Description,
                request.RequiredSkillIds),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [Authorize(Roles = RoleNames.Employer)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateJobRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new UpdateJobCommand(
                UserId,
                id,
                request.Title,
                request.Description,
                request.RequiredSkillIds),
            cancellationToken);

        return Ok(result);
    }

    [Authorize(Roles = RoleNames.Employer)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteJobCommand(UserId, id),
            cancellationToken);

        return NoContent();
    }
}
