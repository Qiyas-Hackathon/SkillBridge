using Microsoft.EntityFrameworkCore;
using SkillBridge.Domain.Entities;

namespace SkillBridge.Infrastructure.Context;

public class SkillBridgeDbContext : DbContext
{
    public SkillBridgeDbContext(DbContextOptions<SkillBridgeDbContext> options)
        : base(options)
    {
    }

    public DbSet<CandidateProfile> CandidateProfiles => Set<CandidateProfile>();
    public DbSet<EmployerProfile> EmployerProfiles => Set<EmployerProfile>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobRequiredSkill> JobRequiredSkills => Set<JobRequiredSkill>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SkillBridgeDbContext).Assembly);
    }
}