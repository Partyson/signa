using System.Data;
using FluentValidation;
using signa.Dto.tournament;

namespace signa.Validators;

public class TournamentValidator : AbstractValidator<CreateTournamentDto>
{
    public TournamentValidator()
    {
        RuleFor(t => t.Title)
            .Matches(@"^.{1,64}$").WithMessage("Неверный формат названия турнира.");
        
        RuleFor(t => t.SportType)
            .Matches(@"^[а-яё]+$").WithMessage("Неверный формат названия вида спорта.");

        RuleFor(t => t.StartedAt)
            .Must(startedAt => startedAt > DateTime.Now).WithMessage("Неверная дата начала турнира");
        
        RuleFor(t => t.EndRegistrationAt)
            .Must((t, endRegistrationAt) =>
                endRegistrationAt > DateTime.Now && t.StartedAt > endRegistrationAt)
            .WithMessage("Неверная дата конца регистрации на турнир");
        
        RuleFor(t => t.ChatLink)
            .Matches(@"^.{1,64}$") // TODO: заглушка, валидировать иначе
            .When(x => x.ChatLink != "")
            .WithMessage("Неверно указана ссылка на беседу вк.");

        RuleFor(t => t.MinFemaleCount)
            .Must((t, minFemaleCount) =>
                minFemaleCount + t.MinMaleCount == t.TeamsMembersMinNumber)
            .WithMessage("Сумма минимального кол-ва парней и девушек не равна минимальному кол-ву участников в команде.");

        RuleFor(t => t.TeamsMembersMaxNumber)
            .Must(teamsMembersMaxNumber =>
                teamsMembersMaxNumber > 0)
            .WithMessage("Максимальное число участников в команде должно быть больше нуля.");

        RuleFor(t => t.TeamsMembersMinNumber)
            .Must((t, teamsMembersMinNumber) =>
                teamsMembersMinNumber < t.TeamsMembersMaxNumber)
            .WithMessage("Минимальное число участников в команде больше максимального.")
            .Must(teamsMembersMinNumber => 
                teamsMembersMinNumber >= 0)
            .WithMessage("Минимальное число участников в команде должно быть больше или равно нулю.");

        RuleFor(t => t.MaxTeamsCount)
            .Must((maxTeamsCount) =>
                maxTeamsCount >= 0) // Если 0 - считаем, что число команд неограничено 
            .WithMessage("Максимальное количество команд должно быть больше или равно нулю.");
        
        RuleFor(t => t.Location)
            .Matches(@"^.{1,64}$").WithMessage("Неверный формат места проведения турнира.");
    }
}