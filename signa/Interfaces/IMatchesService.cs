using signa.Dto.match;

namespace signa.Interfaces;

public interface IMatchesService
{
    Task<List<Guid>> CreateMatchesForTournament(Guid tournamentId);
    Task<Guid> UpdateResult(Guid matchId, UpdateMatchResultDto updateMatchResultDto);
    Task<List<Guid>> SwapTeams(Guid tournamentId, MatchTeamDto matchTeam1, MatchTeamDto matchTeam2);
}
