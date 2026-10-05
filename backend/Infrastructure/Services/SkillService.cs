using Microsoft.EntityFrameworkCore;
using SkillBridge.Application.Interfaces;
using SkillBridge.Domain.Entities;
using SkillBridge.Infrastructure.Context;

namespace SkillBridge.Application.Services;

public class SkillService : ISkillService
{
    private readonly SkillBridgeDbContext _dbContext;

    public SkillService(SkillBridgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Skill>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Skills
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Skill> CreateAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        var exists = await _dbContext.Skills
            .AnyAsync(
                x => x.Name.ToLower() == normalizedName.ToLower(),
                cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException(
                $"Skill '{normalizedName}' already exists.");
        }

        var skill = new Skill
        {
            Name = normalizedName
        };

        _dbContext.Skills.Add(skill);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return skill;
    }
}