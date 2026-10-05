using Microsoft.AspNetCore.Identity;

namespace SkillBridge.Infrastructure.Identity;



public class ApplicationUser : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

      public string? ProfilePicturePath { get; set; }
}
