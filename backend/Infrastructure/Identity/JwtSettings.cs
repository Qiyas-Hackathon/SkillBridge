namespace SkillBridge.Infrastructure.Identity;

/// <summary>Bound from the "Jwt" section of appsettings.json.</summary>
public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

    /// <summary>HMAC-SHA256 signing key. Must be at least 32 characters.</summary>
    public string SecretKey { get; set; } = string.Empty;

    public int ExpiryMinutes { get; set; } = 60;
}
