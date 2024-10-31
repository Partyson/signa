using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using signa.Interfaces;

namespace signa.FunctionalTests.Services.MatchesService;

public class MatchesServiceTestBase : TestBase
{
    protected readonly IUsersService usersService;
    protected readonly IUnitOfWork unitOfWork;
    public MatchesServiceTestBase()
    {
        usersService = Container.GetService<IUsersService>()!;
        unitOfWork = Container.GetService<IUnitOfWork>()!;
    }
}