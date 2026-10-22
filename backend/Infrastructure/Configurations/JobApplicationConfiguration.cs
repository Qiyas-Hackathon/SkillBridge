using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillBridge.Domain.Entities;

namespace SkillBridge.Infrastructure.Configurations;

// Picked up by ApplyConfigurationsFromAssembly.
public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> b)
    {
        b.ToTable("JobApplications");
        b.HasKey(a => a.Id);
        b.Property(a => a.CoverLetter).HasMaxLength(4000);
        b.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);

        // One application per candidate per job.
        b.HasIndex(a => new { a.JobId, a.CandidateProfileId }).IsUnique();

        b.HasOne(a => a.Job).WithMany(j => j.Applications)
            .HasForeignKey(a => a.JobId).OnDelete(DeleteBehavior.Cascade);

        // Restrict avoids SQL Server "multiple cascade paths" (Users -> both profiles -> JobApplications).
        b.HasOne(a => a.CandidateProfile).WithMany()
            .HasForeignKey(a => a.CandidateProfileId).OnDelete(DeleteBehavior.Restrict);
    }
}
