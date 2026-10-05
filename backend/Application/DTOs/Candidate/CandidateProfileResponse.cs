namespace SkillBridge.Application.DTOs.Candidate;

public class CandidateProfileResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Headline { get; set; }
    public string Institution { get; set; } = string.Empty;
    public string? FieldOfStudy { get; set; }
    public string? DegreeLevel { get; set; }
    public int? GraduationYear { get; set; }
    public string? GitHubUrl { get; set; }
    public string? PortfolioUrl { get; set; }

    public List<SkillResponse> Skills { get; set; } = new();
}

public class SkillResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}