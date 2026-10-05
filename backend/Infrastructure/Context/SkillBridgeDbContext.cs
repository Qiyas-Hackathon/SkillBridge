using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillBridge.Domain.Entities;
using SkillBridge.Infrastructure.Identity;

namespace SkillBridge.Infrastructure.Context;

public class SkillBridgeDbContext(
    DbContextOptions<SkillBridgeDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)
{
    public DbSet<CandidateProfile> CandidateProfiles => Set<CandidateProfile>();

    public DbSet<EmployerProfile> EmployerProfiles => Set<EmployerProfile>();

    public DbSet<Skill> Skills => Set<Skill>();

    public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();

    public DbSet<Job> Jobs => Set<Job>();

    public DbSet<JobRequiredSkill> JobRequiredSkills => Set<JobRequiredSkill>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityRole<int>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

        builder.ApplyConfigurationsFromAssembly(
            typeof(SkillBridgeDbContext).Assembly);

        builder.Entity<CandidateProfile>()
.HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<EmployerProfile>()
            .HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<Job>()
            .HasQueryFilter(x => !x.IsDeleted);
    }
}