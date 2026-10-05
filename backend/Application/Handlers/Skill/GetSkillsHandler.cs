using MediatR;
using SkillBridge.Application.DTOs.Skills;
using SkillBridge.Application.Interfaces;
using SkillBridge.Application.Queries.Skill;

namespace SkillBridge.Application.Handlers.Skill;

public class GetSkillsHandler
    : IRequestHandler<GetSkillsQuery, List<SkillResponse>>
{
    private readonly ISkillService _service;

    public GetSkillsHandler(ISkillService service)
    {
        _service = service;
    }

    public async Task<List<SkillResponse>> Handle(
        GetSkillsQuery request,
        CancellationToken cancellationToken)
    {
        var skills = await _service.GetAllAsync(cancellationToken);

        return skills.Select(x => new SkillResponse
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }
}