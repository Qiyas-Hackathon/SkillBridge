using System.ComponentModel.DataAnnotations;
using SkillBridge.Application.Commands.Auth;
using SkillBridge.Application.DTOs.Auth;
using SkillBridge.Domain.Enums;

namespace SkillBridge.Application.DTOs.Auth;




public sealed record RegisterRequest(
    string Email,
    string Password,
    UserRole Role,
    CandidateRegistrationRequest? Candidate,
    EmployerRegistrationRequest? Employer)
{
    public RegisterCommand ToCommand() => new(
        Email,
        Password,
        Role,
        Candidate is null ? null : new CandidateRegistrationData(
            Candidate.FullName,
            Candidate.Headline,
            Candidate.Institution,
            Candidate.FieldOfStudy,
            Candidate.DegreeLevel,
            Candidate.GraduationYear,
            Candidate.GitHubUrl,
            Candidate.PortfolioUrl),
        Employer is null ? null : new EmployerRegistrationData(
            Employer.CompanyName,
            Employer.ContactName,
            Employer.Website));
}


public sealed record CandidateRegistrationRequest(
    string FullName,
    string? Headline,
    string Institution,
    string FieldOfStudy,
    string DegreeLevel,
    int GraduationYear,
    [property: Url] string? GitHubUrl,
    [property: Url] string? PortfolioUrl);

public sealed record EmployerRegistrationRequest(
    string CompanyName,
    string ContactName,
    [property: Url] string? Website);

public sealed record LoginRequest(string Email, string Password);
