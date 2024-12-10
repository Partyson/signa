using System.Data;
using FluentValidation;
using signa.Dto.tournament;

namespace signa.Validators;

public class TournamentValidator : AbstractValidator<CreateTournamentDto>
{
    public TournamentValidator()
    {
        RuleFor(t => t.Title)
            .Matches(@"^[А-Яа-яёЁ\s]+$").WithMessage("Неверный формат названия турнира.");
        
        RuleFor(t => t.SportType)
            .Matches(@"^[а-яё]+$").WithMessage("Неверный формат названия вида спорта.");

        RuleFor(t => t.StartedAt)
            .Must(startedAt => startedAt > DateTime.Now).WithMessage("Неверная дата начала турнира");
        
        RuleFor(t => t.EndRegistrationAt)
            .Must((t, endRegistrationAt) =>
                endRegistrationAt > DateTime.Now && t.StartedAt > endRegistrationAt)
            .WithMessage("Неверная дата конца регистрации на турнир");
        
        RuleFor(t => t.ChatLink)
            .Matches(@"^(https?:\/\/)?(www\.)?vk\.com\/join\/[a-zA-Z0-9_.]+$")
            .When(x => x.ChatLink != "")
            .WithMessage("Неверно указана ссылка на беседу вк.");

        RuleFor(t => t.MinFemaleCount)
            .Must((t, minFemaleCount) =>
                minFemaleCount + t.MinMaleCount == t.TeamsMembersMinNumber)
            .WithMessage("Сумма минимального кол-ва парней и девушек не равна минимальному кол-ву участников в команде.");

        RuleFor(t => t.TeamsMembersMinNumber)
            .Must((t, teamsMembersMinNumber) =>
                teamsMembersMinNumber < t.TeamsMembersMaxNumber);
        
        RuleFor(t => t.Location)
            .Matches(@"^[А-Яа-яёЁ\s]+$").WithMessage("Неверный формат места проведения турнира.");
    }
}