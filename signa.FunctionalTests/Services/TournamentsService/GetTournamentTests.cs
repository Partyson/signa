using FluentAssertions;
using signa.Dto.tournament;

namespace signa.FunctionalTests.Services.TournamentsService;

public class GetTournamentTests : TournamentServiceTestBase
{
    [Test]
    public async Task Should_success_get_all_tournaments()
    {
        var tournaments = await tournamentsService.GetAllTournaments();
        var createdTournamentId = await UltraMegaSigmaGigaHelper.TestCreateTournament();
        
        var currentTournaments = await tournamentsService.GetAllTournaments();
        
        currentTournaments.Count.Should().Be(tournaments.Count + 1);
    }

    [Test]
    public async Task Should_success_get_tournament_entity()
    {
        var createdTournamentId = await UltraMegaSigmaGigaHelper.TestCreateTournament();
        
        var foundTournament = await tournamentsService.GetTournament(createdTournamentId);
        
        foundTournament.Id.Should().Be(createdTournamentId);
    }
    
    [Test]
    public async Task Should_success_get_tournament_response()
    {
        var createdTournamentId = await UltraMegaSigmaGigaHelper.TestCreateTournament();
        var foundTournamentEntity = await tournamentsService.GetTournament(createdTournamentId);
        
        var foundTournamentResponse = await tournamentsService.GetTournamentResponse(createdTournamentId);
        
        foundTournamentEntity.Teams.Select(t => t.Id)
            .Should().BeEquivalentTo(foundTournamentResponse.Teams.Select(t => t.Id));
        foundTournamentEntity.Teams.SelectMany(t => t.Members.Select(m => m.Id))
            .Should().BeEquivalentTo(foundTournamentResponse.Members.Select(m => m.Id));
        foundTournamentEntity.Teams.SelectMany(t => t.Members).Count()
            .Should().Be(foundTournamentResponse.Members.Count);
    }

    [TearDown]
    public async Task TearDown()
    {
        foreach (var id in tournamentToDelete)
            await tournamentsService.DeleteTournament(id);
        
        tournamentToDelete.Clear();
        await unitOfWork.SaveChangesAsync();
    }
}