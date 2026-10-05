namespace SkillBridge.Application.Interfaces;





public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    int? UserId { get; }
    string? Email { get; }
    string? Role { get; }
}
