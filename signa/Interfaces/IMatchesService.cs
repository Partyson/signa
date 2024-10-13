using signa.Dto.match;

namespace signa.Interfaces;

public interface IMatchesService
{
    Task<List<Guid>> CreateMatchesForTournament(Guid tournamentId);
    Task<List<MatchResponseDto>> GetMatchesForTournament(Guid tournamentId);
}
