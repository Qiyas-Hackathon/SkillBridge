using MediatR;

namespace SkillBridge.Application.Commands.Candidate;

public record AddCandidateSkillCommand(
    int UserId,
    int SkillId
) : IRequest<Unit>;