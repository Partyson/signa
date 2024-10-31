using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using signa.Interfaces;

namespace signa.FunctionalTests.Services.MatchesService;

public class MatchesServiceTestBase : TestBase
{
    protected readonly IMatchesService matchesService;
    protected readonly IUnitOfWork unitOfWork;
    public MatchesServiceTestBase()
    {
        matchesService = Container.GetService<IMatchesService>()!;
        unitOfWork = Container.GetService<IUnitOfWork>()!;
    }
}