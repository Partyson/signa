using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using signa.Interfaces;
using signa.Interfaces.Services;

namespace signa.FunctionalTests.Services.TournamentsService;

public class TournamentServiceTestBase : TestBase
{
    protected readonly ITournamentsService tournamentsService;
    protected readonly IUnitOfWork unitOfWork;
    protected List<Guid> tournamentToDelete = new ();
    public TournamentServiceTestBase()
    {
        tournamentsService = Container.GetService<ITournamentsService>()!;
        unitOfWork = Container.GetService<IUnitOfWork>()!;
    }
}