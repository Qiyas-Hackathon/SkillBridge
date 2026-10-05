namespace SkillBridge.Application.Interfaces;

public interface IEmployerProfileService
{
    Task<object?> GetMyProfileAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<object> CreateAsync(
        int userId,
        string companyName,
        string? description,
        CancellationToken cancellationToken = default);

    Task<object> UpdateAsync(
        int userId,
        string companyName,
        string? description,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int userId,
        CancellationToken cancellationToken = default);
}