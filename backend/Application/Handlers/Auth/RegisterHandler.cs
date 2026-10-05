using MediatR;
using SkillBridge.Application.Commands.Auth;
using SkillBridge.Application.DTOs.Auth;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Handlers.Auth;


public sealed class RegisterHandler(IAuthService authService, IJwtTokenService jwtTokenService)
    : IRequestHandler<RegisterCommand, AuthResult>
{
    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await authService.RegisterAsync(
            request with { Email = request.Email.Trim() }, cancellationToken);

        var token = jwtTokenService.GenerateToken(user);

        return new AuthResult(token.AccessToken, token.ExpiresAtUtc, user);
    }
}
