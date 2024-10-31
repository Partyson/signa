using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using signa.Interfaces;

namespace signa.FunctionalTests.Services.TeamsService;

public class TeamsServiceTestBase : TestBase
{
    protected readonly IUsersService usersService;
    protected readonly IUnitOfWork unitOfWork;
    public TeamsServiceTestBase()
    {
        usersService = Container.GetService<IUsersService>()!;
        unitOfWork = Container.GetService<IUnitOfWork>()!;
    }
}