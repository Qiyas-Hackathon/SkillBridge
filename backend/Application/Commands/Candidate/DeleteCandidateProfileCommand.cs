using MediatR;

namespace SkillBridge.Application.Commands.Candidate;

public record DeleteCandidateProfileCommand(
    int UserId
) : IRequest<Unit>;