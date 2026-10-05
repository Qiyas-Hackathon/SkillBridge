using MediatR;
using SkillBridge.Application.DTOs.Candidate;

namespace SkillBridge.Application.Commands.Candidate;

public record CreateCandidateProfileCommand(
    int UserId,
    string FullName,
    string Institution,
    string? Headline,
    string? FieldOfStudy,
    string? DegreeLevel,
    int? GraduationYear,
    string? GitHubUrl,
    string? PortfolioUrl
) : IRequest<CandidateProfileResponse>;