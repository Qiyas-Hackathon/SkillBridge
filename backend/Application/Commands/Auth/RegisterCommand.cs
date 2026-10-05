using MediatR;
using SkillBridge.Application.DTOs.Auth;
using SkillBridge.Domain.Enums;

namespace SkillBridge.Application.Commands.Auth;





public sealed record RegisterCommand(
    string Email,
    string Password,
    UserRole Role,
    CandidateRegistrationData? Candidate,
    EmployerRegistrationData? Employer) : IRequest<AuthResult>;
