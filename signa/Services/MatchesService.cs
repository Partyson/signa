using System.Text.RegularExpressions;
using Mapster;
using signa.Dto.match;
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

    public async Task<List<Guid>> CreateMatchesForTournament(Guid tournamentId)
    {
        var matchesId = await matchRepository.CreateMatches(tournamentId);
        return matchesId;
    }

    public async Task<List<MatchResponseDto>> GetMatchesForTournament(Guid tournamentId)
    {
        var matchesEntity = await matchRepository.GetMatches(tournamentId);
        return matchesEntity.Select(m => m.Adapt<MatchResponseDto>()).ToList();
    }
}