using MediatR;
using SkillBridge.Application.DTOs.Auth;

namespace SkillBridge.Application.Queries.Auth;


public sealed record GetCurrentUserQuery : IRequest<UserDto>;
