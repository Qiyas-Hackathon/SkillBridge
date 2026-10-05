namespace SkillBridge.Application.Interfaces;

public interface IJobService
{
    Task<object> GetAllAsync(
        int? skillId = null,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<object?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<object> CreateAsync(
        int userId,
        string title,
        string description,
        List<int> requiredSkillIds,
        CancellationToken cancellationToken = default);

    Task<object> UpdateAsync(
        int userId,
        int jobId,
        string title,
        string description,
        List<int> requiredSkillIds,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int userId,
        int jobId,
        CancellationToken cancellationToken = default);
}