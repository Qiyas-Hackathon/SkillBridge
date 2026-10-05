using MediatR;
using SkillBridge.Application.Commands.Job;
using SkillBridge.Application.DTOs.Jobs;
using SkillBridge.Application.Interfaces;
using JobEntity = SkillBridge.Domain.Entities.Job;
using SkillResponseDto = SkillBridge.Application.DTOs.Skills.SkillResponse;

namespace SkillBridge.Application.Handlers.Job;

public class CreateJobHandler
    : IRequestHandler<CreateJobCommand, JobResponse>
{
    private readonly IJobService _service;

    public CreateJobHandler(IJobService service)
    {
        _service = service;
    }

    public async Task<JobResponse> Handle(
        CreateJobCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(
            request.UserId,
            request.Title,
            request.Description,
            request.RequiredSkillIds,
            cancellationToken);

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