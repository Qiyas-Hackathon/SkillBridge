namespace SkillBridge.Domain.Entities;

public class Skill
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<CandidateSkill> CandidateSkills { get; set; }
        = new List<CandidateSkill>();

    public ICollection<JobRequiredSkill> JobRequiredSkills { get; set; }
        = new List<JobRequiredSkill>();
}