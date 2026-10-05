using FluentValidation;
using SkillBridge.Application.Commands.Job;

namespace SkillBridge.Application.Validators;

public class CreateJobValidator
    : AbstractValidator<CreateJobCommand>
{
    public CreateJobValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(5000);

        RuleFor(x => x.RequiredSkillIds)
            .NotEmpty()
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Required skills cannot contain duplicates.");
    }
}