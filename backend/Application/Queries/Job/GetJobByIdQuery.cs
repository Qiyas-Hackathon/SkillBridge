using MediatR;
using SkillBridge.Application.DTOs.Jobs;

namespace SkillBridge.Application.Queries.Job;

public record GetJobByIdQuery(
    int JobId
) : IRequest<JobResponse?>;