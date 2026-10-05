namespace SkillBridge.Application.DTOs.Jobs;

public class UpdateJobRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<int> RequiredSkillIds { get; set; } = new();
}