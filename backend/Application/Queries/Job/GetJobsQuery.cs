using MediatR;
using SkillBridge.Application.DTOs.Jobs;

namespace SkillBridge.Application.Queries.Job;

public record GetJobsQuery(
    int? SkillId,
    string? Search
) : IRequest<List<JobResponse>>;