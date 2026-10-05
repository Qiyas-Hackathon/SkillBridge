using MediatR;
using SkillBridge.Application.DTOs.Skills;

namespace SkillBridge.Application.Commands.Skill;

public record CreateSkillCommand(string Name) : IRequest<SkillResponse>;