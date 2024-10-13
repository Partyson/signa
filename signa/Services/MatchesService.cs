using signa.Interfaces;
using signa.Repositories;

namespace signa.Services;


public class MatchesService : IMatchesService
{
    private readonly IMatchRepository matchRepository;

    public MatchesService(IMatchRepository matchRepository)
    {
        this.matchRepository = matchRepository;
    }

    public async Task<List<Guid>> CreateAllMatches(Guid tournamentId)
    {
        var matchesId = await matchRepository.CreateMatches(tournamentId);
        return matchesId;
    }
}