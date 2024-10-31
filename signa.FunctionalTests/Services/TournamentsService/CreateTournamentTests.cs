using FluentAssertions;
using signa.Dto.tournament;
using signa.FunctionalTests.Services.UsersService;

namespace signa.FunctionalTests.Services.TournamentsService;

public class CreateTournamentTests : TournamentServiceTestBase
{
    [TestCase("male")]
    [TestCase("female")]
    [TestCase("mixed")]
    public async Task Should_success_when_tournament_is_valid(string gender)
    {
        var createTournamentDto = new CreateTournamentDto
        {
            Gender = gender,
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
        tournamentToDelete.Add(newTournamentId);
        var tournament = await tournamentsService.GetTournamentResponse(newTournamentId);
        tournament.Should().NotBeNull();
    }
    
    [TearDown]
    public async Task TearDown()
    {
        foreach (var id in tournamentToDelete)
        {
            await tournamentsService.DeleteTournament(id);
        }
        tournamentToDelete.Clear();
        await unitOfWork.SaveChangesAsync();
    }
}