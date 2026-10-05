using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Infrastructure.Identity;


public sealed class CurrentUserService( IHttpContextAccessor accessor) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public int? UserId =>
        int.TryParse(Principal?.FindFirst("sub")?.Value, out var id) ? id : null;

    public string? Email => Principal?.FindFirst("email")?.Value;

    public string? Role => Principal?.FindFirst("role")?.Value;
}
