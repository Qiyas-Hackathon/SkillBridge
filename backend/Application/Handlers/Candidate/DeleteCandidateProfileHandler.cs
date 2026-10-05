using MediatR;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Commands.Candidate;

public class DeleteCandidateProfileHandler
    : IRequestHandler<DeleteCandidateProfileCommand, Unit>
{
    private readonly ICandidateProfileService _service;

    public DeleteCandidateProfileHandler(ICandidateProfileService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(
        DeleteCandidateProfileCommand request,
        CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(
            request.UserId,
            cancellationToken);

        return Unit.Value;
    }
}