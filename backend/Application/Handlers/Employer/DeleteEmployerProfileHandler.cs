using MediatR;
using SkillBridge.Application.Commands.Employer;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Handlers.Employer;

public class DeleteEmployerProfileHandler
    : IRequestHandler<DeleteEmployerProfileCommand, Unit>
{
    private readonly IEmployerProfileService _service;

    public DeleteEmployerProfileHandler(IEmployerProfileService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(
        DeleteEmployerProfileCommand request,
        CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(
            request.UserId,
            cancellationToken);

        return Unit.Value;
    }
}