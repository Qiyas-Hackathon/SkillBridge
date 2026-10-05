using MediatR;
using SkillBridge.Application.Commands.Candidate;
using SkillBridge.Application.Interfaces;
using SkillBridge.Infrastructure.Repositories;
using SkillBridge.Infrastructure.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(
    typeof(CreateCandidateProfileValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(
    typeof(SkillBridge.Application.Validators.CreateCandidateProfileValidator).Assembly);
 
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(CreateCandidateProfileCommand).Assembly));

builder.Services.AddScoped<ICandidateProfileService, CandidateProfileService>();
builder.Services.AddScoped<IEmployerProfileService, EmployerProfileService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ISkillService, SkillService>();

builder.Services.AddScoped<ISkillRepository, SkillRepository>();
   

builder.Services.AddDbContext<SkillBridgeDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

var app = builder.Build();

app.Run();


