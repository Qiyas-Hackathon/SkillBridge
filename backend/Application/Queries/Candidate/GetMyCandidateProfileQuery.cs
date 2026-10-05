using MediatR;
using SkillBridge.Application.DTOs.Candidate;

namespace SkillBridge.Application.Queries.Candidate;

public record GetMyCandidateProfileQuery(
    int UserId
) : IRequest<CandidateProfileResponse?>;