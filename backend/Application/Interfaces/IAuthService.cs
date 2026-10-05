using SkillBridge.Application.Commands.Auth;
using SkillBridge.Application.DTOs.Auth;

namespace SkillBridge.Application.Interfaces;





public interface IAuthService
{
    
    
    
    
    
    
    Task<UserDto> RegisterAsync(RegisterCommand command, CancellationToken ct = default);

    
    Task<UserDto> ValidateCredentialsAsync(string email, string password, CancellationToken ct = default);

    Task<UserDto?> GetByIdAsync(int userId, CancellationToken ct = default);
}
