using MediatR;
using SkillBridge.Application.Commands.Employer;
using SkillBridge.Application.DTOs.Employer;
using SkillBridge.Application.Interfaces;
using SkillBridge.Domain.Entities;

namespace SkillBridge.Application.Handlers.Employer;

public class UpdateEmployerProfileHandler
    : IRequestHandler<UpdateEmployerProfileCommand, EmployerProfileResponse>
{
    private readonly IEmployerProfileService _service;

    public UpdateEmployerProfileHandler(IEmployerProfileService service)
    {
        _service = service;
    }

    public async Task<EmployerProfileResponse> Handle(
        UpdateEmployerProfileCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(
            request.UserId,
            request.CompanyName,
            request.Description,
            cancellationToken);

        var profile = (EmployerProfile)result;

        return new EmployerProfileResponse
        {
            Id = profile.Id,
            UserId = profile.UserId,
            CompanyName = profile.CompanyName,
            Description = profile.Description
        };
    }
}