namespace SkillBridge.Domain.Entities;

public class JobRequiredSkill
{
    public int JobId { get; set; }

    public int SkillId { get; set; }

    public Job Job { get; set; } = null!;

    public Skill Skill { get; set; } = null!;
}