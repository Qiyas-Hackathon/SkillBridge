using Microsoft.EntityFrameworkCore;
using SkillBridge.Domain.Entities;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Infrastructure.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly SkillBridgeDbContext _db;

    public SkillRepository(SkillBridgeDbContext db)
    {
        _db = db;
    }

    public Task<List<Skill>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _db.Skills
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Skill?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _db.Skills
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Skill?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return _db.Skills
            .FirstOrDefaultAsync(
                x => x.Name.ToLower() == name.ToLower(),
                cancellationToken);
    }

    public async Task AddAsync(
        Skill skill,
        CancellationToken cancellationToken = default)
    {
        await _db.Skills.AddAsync(skill, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }
}