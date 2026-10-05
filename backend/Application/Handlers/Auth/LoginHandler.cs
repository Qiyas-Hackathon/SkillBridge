using MediatR;
using SkillBridge.Application.Commands.Auth;
using SkillBridge.Application.DTOs.Auth;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Handlers.Auth;

public sealed class LoginHandler(IAuthService authService, IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, AuthResult>
{
    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await authService.ValidateCredentialsAsync(
            request.Email.Trim(), request.Password, cancellationToken);

        var token = jwtTokenService.GenerateToken(user);

        return new AuthResult(token.AccessToken, token.ExpiresAtUtc, user);
    }
}
