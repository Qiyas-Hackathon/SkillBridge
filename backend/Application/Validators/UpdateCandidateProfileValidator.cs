using FluentValidation;
using SkillBridge.Application.Commands.Candidate;

namespace SkillBridge.Application.Validators;

public class UpdateCandidateProfileValidator
    : AbstractValidator<UpdateCandidateProfileCommand>
{
    public UpdateCandidateProfileValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Institution)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Headline)
            .MaximumLength(200);

        RuleFor(x => x.FieldOfStudy)
            .MaximumLength(150);

        RuleFor(x => x.DegreeLevel)
            .MaximumLength(100);

        RuleFor(x => x.GraduationYear)
            .InclusiveBetween(2000, 2100)
            .When(x => x.GraduationYear.HasValue);

        RuleFor(x => x.GitHubUrl)
            .MaximumLength(500);

        RuleFor(x => x.PortfolioUrl)
            .MaximumLength(500);
    }
}