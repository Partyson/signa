using EntityFrameworkCore.UnitOfWork.Interfaces;
using Quartz;
using signa.Interfaces.Services;

namespace signa.Jobs;

public class MatchesJob : IJob
{
    private readonly IMatchesService matchesService;
    private readonly ITournamentsService tournamentsService;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<MatchesJob> logger;
    
    public MatchesJob(IMatchesService matchesService, ITournamentsService tournamentsService, IUnitOfWork unitOfWork, ILogger<MatchesJob> logger)
    {
        this.matchesService = matchesService;
        this.tournamentsService = tournamentsService;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("MatchesJob started");
        
        var tournaments = await tournamentsService.GetAllTournaments();

        foreach (var tournament in tournaments)
        {
            var matches = await matchesService.GetMatchesByTournamentId(tournament.Id); // найдем матчи, если ошибка - матчей нет
            if (matches.IsError && DateTime.Now > tournament.EndRegistrationAt)
            {
                var result = await matchesService.CreateMatchesForTournament(tournament.Id);

                if (result.IsError)
                {
                    logger.LogError($"MatchesJob: Error on {tournament.Id} tournament when tried to create matches");
                    continue;
                }
                
                await unitOfWork.SaveChangesAsync();
                logger.LogInformation($"MatchesJob: Created matches for tournament {tournament.Id}");
            }
        }
        logger.LogInformation("MatchesJob completed");
    }
}