namespace SkillBridge.Application.DTOs.Employer;

public class UpdateEmployerProfileRequest
{
    public string CompanyName { get; set; } = string.Empty;
    public string? Description { get; set; }
}