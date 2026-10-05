namespace SkillBridge.Application.Interfaces;

public interface ICandidateProfileService
{
    Task<object?> GetMyProfileAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<object> CreateAsync(
        int userId,
        string fullName,
        string institution,
        string? headline,
        string? fieldOfStudy,
        string? degreeLevel,
        int? graduationYear,
        string? githubUrl,
        string? portfolioUrl,
        CancellationToken cancellationToken = default);

    Task<object> UpdateAsync(
        int userId,
        string fullName,
        string institution,
        string? headline,
        string? fieldOfStudy,
        string? degreeLevel,
        int? graduationYear,
        string? githubUrl,
        string? portfolioUrl,
        CancellationToken cancellationToken = default);

    Task AddSkillAsync(
        int userId,
        int skillId,
        CancellationToken cancellationToken = default);

    Task RemoveSkillAsync(
        int userId,
        int skillId,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int userId,
        CancellationToken cancellationToken = default);
}