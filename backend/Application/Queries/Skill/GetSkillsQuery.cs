using MediatR;
using SkillBridge.Application.DTOs.Skills;

namespace SkillBridge.Application.Queries.Skill;

public record GetSkillsQuery : IRequest<List<SkillResponse>>;