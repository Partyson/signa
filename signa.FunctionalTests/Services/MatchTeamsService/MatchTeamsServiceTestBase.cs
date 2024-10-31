using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using signa.Interfaces;

namespace signa.FunctionalTests.Services.MatchTeamsService;

public class MatchTeamsServiceTestBase : TestBase
{
    protected readonly IUsersService usersService;
    protected readonly IUnitOfWork unitOfWork;
    public MatchTeamsServiceTestBase()
    {
        usersService = Container.GetService<IUsersService>()!;
        unitOfWork = Container.GetService<IUnitOfWork>()!;
    }
}