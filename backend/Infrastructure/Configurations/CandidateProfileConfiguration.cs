using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillBridge.Domain.Entities;

namespace SkillBridge.Infrastructure.Configurations;

public class CandidateProfileConfiguration : IEntityTypeConfiguration<CandidateProfile>
{
    public void Configure(EntityTypeBuilder<CandidateProfile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Headline)
            .HasMaxLength(200);

        builder.Property(x => x.Institution) 
            .HasMaxLength(200);

        builder.Property(x => x.FieldOfStudy) 
            .HasMaxLength(150);

        builder.Property(x => x.EducationLevel) 
            .HasMaxLength(100);

        builder.Property(x => x.GitHubUrl)
            .HasMaxLength(500);

        builder.Property(x => x.PortfolioUrl)
            .HasMaxLength(500);

        builder.Property(x => x.CvPath)
            .HasMaxLength(500);

        builder.Property(x => x.ProfilePicturePath)
            .HasMaxLength(500);
    }
}