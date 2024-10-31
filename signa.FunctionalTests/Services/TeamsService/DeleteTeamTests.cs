using FluentAssertions;
using signa.Dto.team;

namespace signa.FunctionalTests.Services.TeamsService;

public class DeleteTeamTests : TeamsServiceTestBase
{
    [Test]
    public async Task Should_success_delete_team()
    {
        var createdTeamId = await TestCreateTeam();
        
        var deletedTeamId = await teamService.DeleteTeam(createdTeamId);
        unitOfWork.SaveChanges();
        var tournamentsTeams = await teamService.GetTeamsByTournamentId(new Guid("08e546bf-ba44-4492-b107-3a740fcc9098"));

        tournamentsTeams.Select(t => t.Id).Contains(deletedTeamId).Should().BeFalse();
    }
    
    private async Task<Guid> TestCreateTeam()
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
        return createdTeamId;
    }
}