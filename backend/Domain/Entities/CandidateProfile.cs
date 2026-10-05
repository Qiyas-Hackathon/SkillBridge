namespace SkillBridge.Domain.Entities;

public class CandidateProfile
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? Headline { get; set; }

    public string? Institution { get; set; } = string.Empty;

    public string? FieldOfStudy { get; set; } = string.Empty;

    public string? EducationLevel { get; set; } = string.Empty;

    public int? GraduationYear { get; set; }

    public string? GitHubUrl { get; set; }

    public string? PortfolioUrl { get; set; }

    public string? CvPath { get; set; }

    public string? ProfilePicturePath { get; set; }

    public ICollection<CandidateSkill> CandidateSkills { get; set; }
        = new List<CandidateSkill>();
}