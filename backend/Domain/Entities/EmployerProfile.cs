namespace SkillBridge.Domain.Entities;

public class EmployerProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoPath { get; set; }
    public bool IsDeleted { get; set; } = false;

    // ---- NEW ----
    public string? ContactName { get; set; }
    public string? Website { get; set; }
    // -------------

    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
