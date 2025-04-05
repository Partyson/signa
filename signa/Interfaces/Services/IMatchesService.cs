using signa.Dto.match;
using ErrorOr;

namespace signa.Interfaces.Services;

public interface IMatchesService
{
    Task<ErrorOr<List<MatchResponseDto>>> GetMatchesByTournamentId(Guid tournamentId);
    Task<ErrorOr<List<Guid>>> CreateMatchesForTournament(Guid tournamentId);
    Task<ErrorOr<Guid>> UpdateResult(Guid matchId, UpdateMatchResultDto updateMatchResultDto);
    Task<ErrorOr<List<Guid>>> SwapTeams(MatchTeamDto matchTeam1, MatchTeamDto matchTeam2);
    Task<ErrorOr<Guid>> FinishMatch(Guid matchId);
}
