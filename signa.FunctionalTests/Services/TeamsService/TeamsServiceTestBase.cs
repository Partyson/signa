using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using signa.Interfaces;
using signa.Interfaces.Services;

namespace signa.FunctionalTests.Services.TeamsService;

public class TeamsServiceTestBase : TestBase
{
    protected readonly ITeamsService teamService;
    protected readonly ITournamentsService tournamentsService;
    protected readonly IUnitOfWork unitOfWork;
    protected readonly List<Guid> teamsToDelete = [];
    public TeamsServiceTestBase()
    {
        teamService = Container.GetService<ITeamsService>()!;
        tournamentsService = Container.GetService<ITournamentsService>()!;
        unitOfWork = Container.GetService<IUnitOfWork>()!;
    }
}