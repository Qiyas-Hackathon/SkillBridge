using MediatR;
using SkillBridge.Application.Interfaces;

namespace SkillBridge.Application.Queries.Candidate;

public class GetMyCandidateProfileHandler
    : IRequestHandler<GetMyCandidateProfileQuery,
        DTOs.Candidate.CandidateProfileResponse?>
{
    private readonly ICandidateProfileService _service;

    public GetMyCandidateProfileHandler(
        ICandidateProfileService service)
    {
        _service = service;
    }

    public async Task<DTOs.Candidate.CandidateProfileResponse?> Handle(
        GetMyCandidateProfileQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetMyProfileAsync(
            request.UserId,
            cancellationToken);

        if (result is null)
            return null;

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