using SkillBridge.Domain.Entities;

namespace SkillBridge.Infrastructure.Repositories;

public interface ISkillRepository
{
    Task<List<Skill>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Skill?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Skill?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Skill skill,
        CancellationToken cancellationToken = default);
}