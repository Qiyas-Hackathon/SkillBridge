using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillBridge.Infrastructure.Identity;

namespace SkillBridge.Infrastructure.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ProfilePicturePath)
            .HasMaxLength(500);
    }
}
