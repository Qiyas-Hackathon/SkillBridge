using SkillBridge.Application.DTOs.Auth;

namespace SkillBridge.Application.Interfaces;

public interface IJwtTokenService
{
    TokenResult GenerateToken(UserDto user);
}
