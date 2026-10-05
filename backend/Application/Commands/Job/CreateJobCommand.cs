using MediatR;
using SkillBridge.Application.DTOs.Jobs;

namespace SkillBridge.Application.Commands.Job;

public record CreateJobCommand(
    int UserId,
    string Title,
    string Description,
    List<int> RequiredSkillIds
) : IRequest<JobResponse>;