using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using signa.Interfaces;

namespace signa.FunctionalTests.Services.MatchTeamsService;

public class MatchTeamsServiceTestBase : TestBase
{
    protected readonly IMatchTeamsService matchTeamsService;
    protected readonly IUnitOfWork unitOfWork;
    public MatchTeamsServiceTestBase()
    {
        matchTeamsService = Container.GetService<IMatchTeamsService>()!;
        unitOfWork = Container.GetService<IUnitOfWork>()!;
    }
}