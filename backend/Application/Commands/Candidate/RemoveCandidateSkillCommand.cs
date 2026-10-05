using MediatR;

namespace SkillBridge.Application.Commands.Candidate;

public record RemoveCandidateSkillCommand(
    int UserId,
    int SkillId
) : IRequest<Unit>;