using FluentValidation;
using SkillBridge.Application.Commands.Skill;

namespace SkillBridge.Application.Validators;

public class CreateSkillValidator : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
