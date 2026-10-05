namespace SkillBridge.Domain.Entities;

public class Job
{
    public int Id { get; set; }

    public int EmployerProfileId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public EmployerProfile EmployerProfile { get; set; } = null!;

    public ICollection<JobRequiredSkill> RequiredSkills { get; set; }
        = new List<JobRequiredSkill>();
}