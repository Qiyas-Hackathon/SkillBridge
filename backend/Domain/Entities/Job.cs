using Microsoft.EntityFrameworkCore;
using SkillBridge.Domain.Enums;

namespace SkillBridge.Domain.Entities;

public class Job
{
    public int Id { get; set; }
    public int EmployerProfileId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public EmployerProfile EmployerProfile { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;

    public string? Location { get; set; }
    public JobType JobType { get; set; } = JobType.FullTime;
    [Precision(12, 2)] public decimal? SalaryMin { get; set; }
    [Precision(12, 2)] public decimal? SalaryMax { get; set; }
    public string? SalaryCurrency { get; set; } = "USD";
    public DateTime? Deadline { get; set; }
    public bool IsActive { get; set; } = true;   // employer "close job" / admin hide
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    // -------------

    public ICollection<JobRequiredSkill> RequiredSkills { get; set; } = new List<JobRequiredSkill>();
}
