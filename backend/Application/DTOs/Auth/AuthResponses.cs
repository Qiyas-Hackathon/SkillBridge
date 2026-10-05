using SkillBridge.Application.DTOs.Auth;

namespace SkillBridge.Api.DTOs.Auth;

public sealed record UserResponse(int Id, string Email, string Role)
{
    public static UserResponse From(UserDto user) => new(user.Id, user.Email, user.Role);
}

public sealed record AuthResponse(string AccessToken, DateTime ExpiresAtUtc, UserResponse User)
{
    public static AuthResponse From(AuthResult result) =>
        new(result.AccessToken, result.ExpiresAtUtc, UserResponse.From(result.User));
}
