using MediatR;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Commands.Candidate;

public class RemoveCandidateSkillHandler
    : IRequestHandler<RemoveCandidateSkillCommand, Unit>
{
    private readonly ICandidateProfileService _service;

    public RemoveCandidateSkillHandler(ICandidateProfileService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(
        RemoveCandidateSkillCommand request,
        CancellationToken cancellationToken)
    {
        await _service.RemoveSkillAsync(
            request.UserId,
            request.SkillId,
            cancellationToken);

        return Unit.Value;
    }
}