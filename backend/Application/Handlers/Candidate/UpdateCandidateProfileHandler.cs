using MediatR;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Commands.Candidate;

public class UpdateCandidateProfileHandler
    : IRequestHandler<UpdateCandidateProfileCommand,
        DTOs.Candidate.CandidateProfileResponse>
{
    private readonly ICandidateProfileService _service;

    public UpdateCandidateProfileHandler(
        ICandidateProfileService service)
    {
        _service = service;
    }

    public async Task<DTOs.Candidate.CandidateProfileResponse> Handle(
        UpdateCandidateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(
            request.UserId,
            request.FullName,
            request.Institution,
            request.Headline,
            request.FieldOfStudy,
            request.DegreeLevel,
            request.GraduationYear,
            request.GitHubUrl,
            request.PortfolioUrl,
            cancellationToken);

        var profile = (Domain.Entities.CandidateProfile)result;

        return new DTOs.Candidate.CandidateProfileResponse
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FullName = profile.FullName,
            Headline = profile.Headline,
            Institution = profile.Institution,
            FieldOfStudy = profile.FieldOfStudy,
            DegreeLevel = profile.DegreeLevel,
            GraduationYear = profile.GraduationYear,
            GitHubUrl = profile.GitHubUrl,
            PortfolioUrl = profile.PortfolioUrl,
            Skills = profile.CandidateSkills.Select(x =>
                new DTOs.Candidate.SkillResponse
                {
                    Id = x.SkillId,
                    Name = x.Skill.Name
                }).ToList()
        };
    }
}