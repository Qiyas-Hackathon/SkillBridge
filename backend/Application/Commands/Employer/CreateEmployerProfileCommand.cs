using MediatR;
using SkillBridge.Application.DTOs.Employer;

namespace SkillBridge.Application.Commands.Employer;

public record CreateEmployerProfileCommand(
    int UserId,
    string CompanyName,
    string? Description
) : IRequest<EmployerProfileResponse>;