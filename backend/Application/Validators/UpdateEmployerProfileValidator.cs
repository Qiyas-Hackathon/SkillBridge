using FluentValidation;
using SkillBridge.Application.Commands.Employer;

namespace SkillBridge.Application.Validators;

public class UpdateEmployerProfileValidator
    : AbstractValidator<UpdateEmployerProfileCommand>
{
    public UpdateEmployerProfileValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}