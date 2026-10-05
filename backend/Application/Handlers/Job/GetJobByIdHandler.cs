using MediatR;
using SkillBridge.Application.DTOs.Jobs;
using SkillBridge.Application.Interfaces;
using SkillBridge.Application.Queries.Job;
using JobEntity = SkillBridge.Domain.Entities.Job;
using SkillResponseDto = SkillBridge.Application.DTOs.Skills.SkillResponse;

namespace SkillBridge.Application.Handlers.Job;

public class GetJobByIdHandler
    : IRequestHandler<GetJobByIdQuery, JobResponse?>
{
    private readonly IJobService _service;

    public GetJobByIdHandler(IJobService service)
    {
        _service = service;
    }

    public async Task<JobResponse?> Handle(
        GetJobByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(
            request.JobId,
            cancellationToken);

        if (result is null)
            return null;

        return Map((JobEntity)result);
    }

    private static JobResponse Map(JobEntity job)
    {
        return new JobResponse
        {
            Id = job.Id,
            EmployerProfileId = job.EmployerProfileId,
            Title = job.Title,
            Description = job.Description,
            CreatedAt = job.CreatedAt,
            CompanyName = job.EmployerProfile?.CompanyName ?? string.Empty,
            RequiredSkills = job.RequiredSkills
                .Select(x => new SkillResponseDto
                {
                    Id = x.Skill.Id,
                    Name = x.Skill.Name
                })
                .ToList()
        };
    }
}