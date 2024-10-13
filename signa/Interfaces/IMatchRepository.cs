using signa.Entities;

namespace signa.Interfaces;

public interface IMatchRepository
{
    Task<List<Guid>> CreateMatches(Guid tournamentId);
    Task<List<MatchEntity>> GetMatches(Guid tournamentId);
}