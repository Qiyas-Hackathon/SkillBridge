using Microsoft.EntityFrameworkCore;
using SkillBridge.Infrastructure.Context;
using SkillBridge.Application.Interfaces;
using SkillBridge.Application.Services;
using SkillBridge.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);


 
builder.Services.AddScoped<ICandidateProfileService, CandidateProfileService>();
builder.Services.AddScoped<IEmployerProfileService, EmployerProfileService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(SkillBridge.Application.Commands.Candidate.CreateCandidateProfileCommand)
            .Assembly));

   

builder.Services.AddDbContext<SkillBridgeDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

var app = builder.Build();

app.Run();


