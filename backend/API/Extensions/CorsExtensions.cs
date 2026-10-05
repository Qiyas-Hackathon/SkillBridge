namespace SkillBridge.Api.Extensions;

public static class CorsExtensions
{
    public const string PolicyName = "AngularClient";

    public static IServiceCollection AddAngularCors(this IServiceCollection services, IConfiguration config)
    {
        var origins = config.GetSection("Cors:AllowedOrigins").Get<string[]>()
                      ?? ["http://localhost:4200"];

        services.AddCors(options =>
            options.AddPolicy(PolicyName, policy =>
                policy.WithOrigins(origins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()));

        return services;
    }
}
