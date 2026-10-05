using SkillBridge.Domain.Entities;

namespace SkillBridge.Application.Interfaces;

public interface ISkillService
{
    Task<List<Skill>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Skill> CreateAsync(
        string name,
        CancellationToken cancellationToken = default);
}