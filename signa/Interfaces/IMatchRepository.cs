using signa.Dto.match;
using signa.Entities;

namespace signa.Interfaces;

public interface IMatchRepository
{
    Task<List<Guid>> CreateMatches(List<MatchEntity> matches);
    Task<Guid> UpdateResults(Guid matchId, UpdateMatchResultDto updateMatchResultDto);
    Task<List<Guid>> SwapTeams(Guid tournamentId, MatchTeamDto matchTeam1, MatchTeamDto matchTeam2);
    Task<Guid> FinishMatch(Guid matchId);
}