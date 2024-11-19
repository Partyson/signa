using FluentAssertions;
using signa.Dto.team;

namespace signa.FunctionalTests.Services.TeamsService;

public class GetTeamTests : TeamsServiceTestBase
{
    [Test]
    public async Task Should_success_get_team()
    {
        var (createdTeamId, createTeamDto) = await TestCreateTeam();
        
        var foundedTeam = await teamService.GetTeam(createdTeamId);
        
        foundedTeam.Id.Should().Be(createdTeamId);
        foundedTeam.Captain.Id.Should().Be(createTeamDto.CaptainId);
        foundedTeam.Members.Select(m => m.Id).Should().BeEquivalentTo(createTeamDto.MembersId);
        foundedTeam.Title.Should().Be(createTeamDto.Title);
    }

    [Test]
    public async Task Should_return_all_team_for_tournament()
    {
        var tournament = await tournamentsService.GetTournament(new Guid("08e546bf-ba44-4492-b107-3a740fcc9098"));
        var tournamentsTeamsIds = tournament.Teams.Select(t => t.Id).ToList();

        var foundedTeamsByTournamentId =
            await teamService.GetTeamsByTournamentId(new Guid("08e546bf-ba44-4492-b107-3a740fcc9098"));
        
        tournamentsTeamsIds.Should().BeEquivalentTo(foundedTeamsByTournamentId.Select(t => t.Id));
    }

    private async Task<(Guid, CreateTeamDto)> TestCreateTeam()
    {
        var createTeamDto = new CreateTeamDto
        {
            CaptainId = new Guid("df1a00d0-a294-4d20-b99d-e99a42d80efe"),
            MembersId = [new Guid("df1a00d0-a294-4d20-b99d-e99a42d80efe")],
            Title = "Чебупели",
            TournamentId = new Guid("08e546bf-ba44-4492-b107-3a740fcc9098")
        };
        
        var createdTeamId = await teamService.CreateTeam(createTeamDto);
        teamsToDelete.Add(createdTeamId);
        unitOfWork.SaveChanges();
        return (createdTeamId, createTeamDto);
    }
    
    [TearDown]
    public async Task TearDown()
    {
        foreach (var id in teamsToDelete)
            await teamService.DeleteTeam(id);
        
        teamsToDelete.Clear();
        await unitOfWork.SaveChangesAsync();
    }
}