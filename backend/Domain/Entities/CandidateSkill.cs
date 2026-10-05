namespace SkillBridge.Domain.Entities;

public class CandidateSkill
{
    public int CandidateProfileId { get; set; }

    public int SkillId { get; set; }

    public CandidateProfile CandidateProfile { get; set; } = null!;

    public Skill Skill { get; set; } = null!;
}