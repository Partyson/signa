using FluentValidation;
using signa.Dto.team;

namespace signa.Validators;

public class TeamValidator : AbstractValidator<CreateTeamDto>
{
    public TeamValidator()
    {
        RuleFor(t => t.Title)
            .Matches(@"^[А-Яа-яёЁ\s]+$").WithMessage("Неверный формат названия команды.");
    }
}