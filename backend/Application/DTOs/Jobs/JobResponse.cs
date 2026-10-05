namespace SkillBridge.Application.DTOs.Jobs;

public class JobResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public int EmployerProfileId { get; set; }
    public string CompanyName { get; set; } = string.Empty;

    public List<SkillResponse> RequiredSkills { get; set; } = new();
}

public class SkillResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}