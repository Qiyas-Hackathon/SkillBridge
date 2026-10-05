using MediatR;

namespace SkillBridge.Application.Commands.Job;

public record DeleteJobCommand(
    int UserId,
    int JobId
) : IRequest<Unit>;