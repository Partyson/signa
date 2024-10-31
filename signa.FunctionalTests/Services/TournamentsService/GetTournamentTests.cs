using FluentAssertions;
using signa.Dto.tournament;

namespace signa.FunctionalTests.Services.TournamentsService;

public class GetTournamentTests : TournamentServiceTestBase
{
    [Test]
    public async Task Should_success_get_all_tournaments()
    {
        var tournaments = await tournamentsService.GetAllTournaments();
        var createdTournamentId = await TestCreateTournament();
        
        var currentTournaments = await tournamentsService.GetAllTournaments();
        
        currentTournaments.Count.Should().Be(tournaments.Count + 1);
    }

    [Test]
    public async Task Should_success_get_tournament_entity()
    {
        var createdTournamentId = await TestCreateTournament();
        
        var foundTournament = await tournamentsService.GetTournament(createdTournamentId);
        
        foundTournament.Id.Should().Be(createdTournamentId);
    }
    
    [Test]
    public async Task Should_success_get_tournament_response()
    {
        var createdTournamentId = await TestCreateTournament();
        var foundTournamentEntity = await tournamentsService.GetTournament(createdTournamentId);
        
        var foundTournamentResponse = await tournamentsService.GetTournamentResponse(createdTournamentId);
        
        foundTournamentEntity.Teams.Select(t => t.Id)
            .Should().BeEquivalentTo(foundTournamentResponse.Teams.Select(t => t.Id));
        foundTournamentEntity.Teams.SelectMany(t => t.Members.Select(m => m.Id))
            .Should().BeEquivalentTo(foundTournamentResponse.Members.Select(m => m.Id));
        foundTournamentEntity.Teams.SelectMany(t => t.Members).Count()
            .Should().Be(foundTournamentResponse.Members.Count);
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
        tournamentToDelete.Add(newTournamentId);
        return newTournamentId;
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