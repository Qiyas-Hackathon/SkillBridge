using System.ComponentModel.DataAnnotations;
using SkillBridge.Domain.Enums;

namespace SkillBridge.Api.Contracts;

public record PagedResult<T>(List<T> Items, int Total, int Page, int PageSize);
public record SkillDto(int Id, string Name);

// ---------- Jobs ----------
public class JobWriteRequest : IValidatableObject
{
    [Required, MaxLength(200)] public string Title { get; set; } = "";
    [Required] public string Description { get; set; } = "";
    public List<int> SkillIds { get; set; } = new();
    [MaxLength(200)] public string? Location { get; set; }
    public JobType JobType { get; set; } = JobType.FullTime;
    [Range(0, double.MaxValue)] public decimal? SalaryMin { get; set; }
    [Range(0, double.MaxValue)] public decimal? SalaryMax { get; set; }
    [MaxLength(3)] public string? SalaryCurrency { get; set; } = "USD";
    public DateTime? Deadline { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        if (SalaryMin > SalaryMax)
            yield return new ValidationResult("SalaryMin cannot exceed SalaryMax.", new[] { nameof(SalaryMin) });
        if (Deadline is not null && Deadline.Value.Date < DateTime.UtcNow.Date)
            yield return new ValidationResult("Deadline cannot be in the past.", new[] { nameof(Deadline) });
    }
}

public record JobDto(int Id, int EmployerProfileId, string CompanyName, string Title, string Description,
    List<SkillDto> RequiredSkills, string? Location, JobType JobType, decimal? SalaryMin, decimal? SalaryMax,
    string? SalaryCurrency, DateTime? Deadline, bool IsActive, DateTime CreatedAt);

// ---------- Employer profile ----------
public class EmployerProfileRequest
{
    [Required, MaxLength(200)] public string CompanyName { get; set; } = "";
    public string? Description { get; set; }
    [MaxLength(150)] public string? ContactName { get; set; }
    [MaxLength(300), Url] public string? Website { get; set; }
}
public record EmployerProfileDto(int Id, string CompanyName, string? Description, string? ContactName,
    string? Website, string? LogoPath);

// ---------- Applications ----------
public class ApplyRequest { [MaxLength(4000)] public string? CoverLetter { get; set; } }
public class UpdateApplicationStatusRequest { [Required] public ApplicationStatus Status { get; set; } }

public record ApplicationDto(int Id, int JobId, string JobTitle, string CompanyName,
    int CandidateProfileId, string CandidateName, string? CandidateEmail, string? Headline,
    string? CvPath, string? GitHubUrl, string? PortfolioUrl, string? CoverLetter,
    ApplicationStatus Status, DateTime AppliedAt, DateTime? UpdatedAt);

// ---------- Admin ----------
public class SetActiveRequest { public bool IsActive { get; set; } }
public record AdminUserDto(int Id, string Email, string Role, string Name, bool IsActive);
public record AdminStatsDto(int TotalUsers, int Candidates, int Employers, int TotalJobs, int ActiveJobs,
    int TotalApplications, Dictionary<string, int> ApplicationsByStatus);
