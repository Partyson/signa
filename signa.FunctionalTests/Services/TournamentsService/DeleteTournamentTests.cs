using FluentAssertions;
using signa.Dto.tournament;

namespace signa.FunctionalTests.Services.TournamentsService;

public class DeleteTournamentTests : TournamentServiceTestBase
{
    [Test]
    public async Task Should_success_delete_tournament()
    {
        var createdTournamentId = await TestCreateTournament();
        
        await tournamentsService.DeleteTournament(createdTournamentId);
        unitOfWork.SaveChanges();
        var tournaments = await tournamentsService.GetAllTournaments();
        
        tournaments.Select(t => t.Id).Contains(createdTournamentId).Should().BeFalse();
    }
    
    private async Task<Guid> TestCreateTournament()
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