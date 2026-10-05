using MediatR;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Commands.Candidate;

public class AddCandidateSkillHandler
    : IRequestHandler<AddCandidateSkillCommand, Unit>
{
    private readonly ICandidateProfileService _service;

    public AddCandidateSkillHandler(ICandidateProfileService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(
        AddCandidateSkillCommand request,
        CancellationToken cancellationToken)
    {
        await _service.AddSkillAsync(
            request.UserId,
            request.SkillId,
            cancellationToken);

        return Unit.Value;
    }
}