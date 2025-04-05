using signa.Dto.team;
using ErrorOr;

namespace signa.Interfaces.Services;

public interface IMatchTeamsService
{
    Task<ErrorOr<Guid>> UpdateResult(Guid matchId, List<UpdateTeamScoreDto> newTeamScores);
    Task<ErrorOr<Guid>> FinishMatch(Guid matchId);
}