using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SkillBridge.Api.Controllers;

public static class ControllerBaseExtensions
{
    // Assumes your JWT puts the Identity user id in the NameIdentifier claim.
    public static int GetUserId(this ControllerBase c) =>
        int.Parse(c.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
