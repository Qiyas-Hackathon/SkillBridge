using MediatR;

namespace SkillBridge.Application.Commands.Employer;

public record DeleteEmployerProfileCommand(int UserId) : IRequest<Unit>;