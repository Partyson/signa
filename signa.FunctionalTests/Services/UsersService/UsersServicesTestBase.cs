using System.ComponentModel;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using signa.Interfaces;

namespace signa.FunctionalTests.Services.UsersService;

public class UsersServicesTestBase : TestBase
{
    protected readonly IUsersService usersService;
    protected readonly IUnitOfWork unitOfWork;
    public UsersServicesTestBase()
    {
        usersService = Container.GetService<IUsersService>()!;
        unitOfWork = Container.GetService<IUnitOfWork>()!;
    }
}