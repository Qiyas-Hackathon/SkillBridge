using MediatR;
using SkillBridge.Application.DTOs.Employer;
using SkillBridge.Application.Interfaces;
using SkillBridge.Application.Queries.Employer;
using SkillBridge.Domain.Entities;

namespace SkillBridge.Application.Handlers.Employer;

public class GetMyEmployerProfileHandler
    : IRequestHandler<GetMyEmployerProfileQuery, EmployerProfileResponse?>
{
    private readonly IEmployerProfileService _service;

    public GetMyEmployerProfileHandler(IEmployerProfileService service)
    {
        _service = service;
    }

    public async Task<EmployerProfileResponse?> Handle(
        GetMyEmployerProfileQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetMyProfileAsync(
            request.UserId,
            cancellationToken);

        if (result is null)
            return null;

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