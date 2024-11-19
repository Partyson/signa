using FluentAssertions;
using signa.Dto.team;

namespace signa.FunctionalTests.Services.TeamsService;

public class CreateTeamTests : TeamsServiceTestBase
{
    [Test]
    public async Task Should_success_when_team_valid()
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
        
        var team = await teamService.GetTeam(createdTeamId);
        
        team.Id.Should().Be(createdTeamId);
        team.Title.Should().Be(createTeamDto.Title);
        team.Members.Should().NotBeEmpty();
        team.Captain.Should().NotBeNull();
        var tournamentsTeam = await teamService.GetTeamsByTournamentId(createTeamDto.TournamentId);
        tournamentsTeam.Select(t => t.Id)
            .Contains(team.Id).Should().BeTrue();
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