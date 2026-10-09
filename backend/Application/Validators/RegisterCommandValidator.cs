using FluentValidation;
using SkillBridge.Application.Commands.Auth;
using SkillBridge.Application.DTOs.Auth;
using SkillBridge.Domain.Enums;

namespace SkillBridge.Application.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(128);

        RuleFor(x => x.Role)
            .IsInEnum();

        When(x => x.Role == UserRole.Candidate, () =>
        {
            RuleFor(x => x.Candidate)
                .NotNull()
                .WithMessage("Candidate details are required when registering as a candidate.");

            RuleFor(x => x.Candidate!)
                .SetValidator(new CandidateDataValidator())
                .When(x => x.Candidate is not null);
        });

        When(x => x.Role == UserRole.Employer, () =>
        {
            RuleFor(x => x.Employer)
                .NotNull()
                .WithMessage("Employer details are required when registering as an employer.");

            RuleFor(x => x.Employer!)
                .SetValidator(new EmployerDataValidator())
                .When(x => x.Employer is not null);
        });
    }

    // Internal on purpose: AddValidatorsFromAssembly only registers public validators,
    // and these are only ever used as child validators.
    private sealed class CandidateDataValidator : AbstractValidator<CandidateRegistrationData>
    {
        public CandidateDataValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Institution).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Headline).MaximumLength(200);
            RuleFor(x => x.FieldOfStudy).NotEmpty().MaximumLength(150);
            RuleFor(x => x.DegreeLevel).NotEmpty().MaximumLength(100);
            RuleFor(x => x.GraduationYear).InclusiveBetween(2000, 2100);
            RuleFor(x => x.GitHubUrl).MaximumLength(500);
            RuleFor(x => x.PortfolioUrl).MaximumLength(500);
        }
    }

    private sealed class EmployerDataValidator : AbstractValidator<EmployerRegistrationData>
    {
        public EmployerDataValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ContactName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Website).MaximumLength(500);
        }
    }
}
