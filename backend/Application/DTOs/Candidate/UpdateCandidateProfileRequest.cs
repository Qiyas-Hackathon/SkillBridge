namespace SkillBridge.Application.DTOs.Candidate;

public class UpdateCandidateProfileRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Institution { get; set; } = string.Empty;
    public string? Headline { get; set; }
    public string? FieldOfStudy { get; set; }
    public string? DegreeLevel { get; set; }
    public int? GraduationYear { get; set; }
    public string? GitHubUrl { get; set; }
    public string? PortfolioUrl { get; set; }
}
