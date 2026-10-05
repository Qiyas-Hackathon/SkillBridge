using MediatR;
using SkillBridge.Application.DTOs.Employer;

namespace SkillBridge.Application.Queries.Employer;

public record GetMyEmployerProfileQuery(int UserId)
    : IRequest<EmployerProfileResponse?>;