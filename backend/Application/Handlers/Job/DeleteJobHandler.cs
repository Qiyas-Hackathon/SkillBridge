using MediatR;
using SkillBridge.Application.Commands.Job;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Handlers.Job;
using Job= SkillBridge.Domain.Entities.Job;
public class DeleteJobHandler
    : IRequestHandler<DeleteJobCommand, Unit>
{
    private readonly IJobService _service;

    public DeleteJobHandler(IJobService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(
        DeleteJobCommand request,
        CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(
            request.UserId,
            request.JobId,
            cancellationToken);

        return Unit.Value;
    }
}