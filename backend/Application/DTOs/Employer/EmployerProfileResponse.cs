namespace SkillBridge.Application.DTOs.Employer;

public class EmployerProfileResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? Description { get; set; }
}