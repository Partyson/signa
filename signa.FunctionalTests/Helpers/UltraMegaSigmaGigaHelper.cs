using EntityFrameworkCore.UnitOfWork.Interfaces;
using signa.Dto.tournament;
using signa.Interfaces;

namespace signa.FunctionalTests.Helpers;

public class UltraMegaSigmaGigaHelper
{
    private readonly ITournamentsService tournamentsService;
    private readonly ITeamsService teamService;
    private readonly IUnitOfWork unitOfWork;

    public UltraMegaSigmaGigaHelper(ITournamentsService tournamentsService, ITeamsService teamService, IUnitOfWork unitOfWork)
    {
        this.tournamentsService = tournamentsService;
        this.teamService = teamService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Guid> TestCreateTournament()
    {
        var createTournamentDto = new CreateTournamentDto
        {
            Gender = "male",
            Location = "ФОК",
            MaxTeamsCount = 16,
            MinFemaleCount = 0,
            MinMaleCount = 0,
            SportType = "волейбол",
            StartedAt = new DateTime(2024, 12, 12, 12, 0, 0),
            EndRegistrationAt = new DateTime(2024, 12, 11, 12, 0, 0),
            Title = "Чемпионат мира по волейболу",
            TeamsMembersMaxNumber = 8,
            State = "registration",
            WithGroupStage = false
        };
        
        var newTournamentId = await tournamentsService.CreateTournament(createTournamentDto);
        unitOfWork.SaveChanges();
        return newTournamentId;
    }
}