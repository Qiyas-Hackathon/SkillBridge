using MediatR;
using SkillBridge.Application.Commands.Skill;
using SkillBridge.Application.DTOs.Skills;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Handlers.Skill;

public class CreateSkillHandler
    : IRequestHandler<CreateSkillCommand, SkillResponse>
{
    private readonly ISkillService _service;

    public CreateSkillHandler(ISkillService service)
    {
        _service = service;
    }

    public async Task<SkillResponse> Handle(
        CreateSkillCommand request,
        CancellationToken cancellationToken)
    {
        var skill = await _service.CreateAsync(
            request.Name,
            cancellationToken);

        return new SkillResponse
        {
            Id = skill.Id,
            Name = skill.Name
        };
    }
}