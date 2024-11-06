using signa.Dto.match;
using signa.Dto.team;

namespace signa.Interfaces;

public interface IMatchTeamsService
{
    Task<Guid> UpdateResult(Guid matchId, List<UpdateTeamScoreDto> newTeamScores);
    Task<Guid> FinishMatch(Guid matchId);
}