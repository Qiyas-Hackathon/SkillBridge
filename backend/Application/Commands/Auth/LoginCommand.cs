using MediatR;
using SkillBridge.Application.DTOs.Auth;

namespace SkillBridge.Application.Commands.Auth;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResult>;
