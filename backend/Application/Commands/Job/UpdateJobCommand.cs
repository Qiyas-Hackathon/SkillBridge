using MediatR;
using SkillBridge.Application.DTOs.Jobs;

namespace SkillBridge.Application.Commands.Job;

public record UpdateJobCommand(
    int UserId,
    int JobId,
    string Title,
    string Description,
    List<int> RequiredSkillIds
) : IRequest<JobResponse>;