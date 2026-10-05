using MediatR;
using SkillBridge.Application.Commands.Job;
using SkillBridge.Application.DTOs.Jobs;
using SkillBridge.Application.Interfaces;
using JobEntity = SkillBridge.Domain.Entities.Job;
using SkillResponseDto = SkillBridge.Application.DTOs.Skills.SkillResponse;

namespace SkillBridge.Application.Handlers.Job;

public class UpdateJobHandler
    : IRequestHandler<UpdateJobCommand, JobResponse>
{
    private readonly IJobService _service;

    public UpdateJobHandler(IJobService service)
    {
        _service = service;
    }

    public async Task<JobResponse> Handle(
        UpdateJobCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(
            request.UserId,
            request.JobId,
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