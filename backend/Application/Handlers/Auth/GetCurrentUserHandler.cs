using MediatR;
using SkillBridge.Application.DTOs.Auth;
using SkillBridge.Application.Exceptions;
using SkillBridge.Application.Interfaces;
using SkillBridge.Application.Queries.Auth;

namespace SkillBridge.Application.Handlers.Auth;

public sealed class GetCurrentUserHandler(IAuthService authService, ICurrentUserService currentUser)
    : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
            throw new AuthenticationFailedException("You are not signed in.");

        return await authService.GetByIdAsync(userId, cancellationToken)
               ?? throw new NotFoundException("User not found.");
    }
}
