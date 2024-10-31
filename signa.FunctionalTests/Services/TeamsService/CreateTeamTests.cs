using signa.Dto.team;
using signa.FunctionalTests.Services.TeamsService;

namespace signa.FunctionalTests.Services.UsersService;

public class CreateTeamTests : TeamsServiceTestBase
{
    [Test]
    public async Task Shoul_success_when_team_valid()
    {
        var createTeamDto = new CreateTeamDto
        {
            CaptainId = new Guid("df1a00d0-a294-4d20-b99d-e99a42d80efe"),
            MembersId = [new Guid("df1a00d0-a294-4d20-b99d-e99a42d80efe")],
            Title = "Чебупели",
            //TournamentId = ""
        };
    }
}