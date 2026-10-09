using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SkillBridge.Api.Extensions;
using SkillBridge.Api.Middleware;
using SkillBridge.Application;
using SkillBridge.Application.Interfaces;
using SkillBridge.Infrastructure;
using SkillBridge.Infrastructure.Context;
using SkillBridge.Infrastructure.Identity;
using SkillBridge.Infrastructure.Repositories;
using SkillBridge.Infrastructure.Services;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration); // DbContext, Identity, JwtSettings

// Needed by CurrentUserService to read the signed-in user's claims.
builder.Services.AddHttpContextAccessor();

// Auth / identity services
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Repositories
builder.Services.AddScoped<ISkillRepository, SkillRepository>();

// Domain services
builder.Services.AddScoped<ICandidateProfileService, CandidateProfileService>();
builder.Services.AddScoped<IEmployerProfileService, EmployerProfileService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ISkillService, SkillService>();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAngularCors(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        // Lets clients send "role": "Candidate" instead of the enum number.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            }
        ];

        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();   

    // Development convenience: apply pending migrations on startup.
    using var migrationScope = app.Services.CreateScope();
    var db = migrationScope.ServiceProvider.GetRequiredService<SkillBridgeDbContext>();
    await db.Database.MigrateAsync();
}

// Registration assigns a role by name
using (var seedScope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedRolesAsync(seedScope.ServiceProvider);
}

// app.UseHttpsRedirection();

app.UseCors(CorsExtensions.PolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();