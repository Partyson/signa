using signa.Dto.match;
using signa.Entities;

namespace signa.Interfaces;

public interface IMatchRepository
{
    Task<List<Guid>> CreateMatches(List<MatchEntity> matches);
    Task<Guid> UpdateResults(Guid matchId, UpdateMatchResultDto updateMatchResultDto);
}