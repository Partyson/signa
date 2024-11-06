using signa.Dto.team;

namespace signa.Interfaces.Services;

public interface IMatchTeamsService
{
    Task<Guid> UpdateResult(Guid matchId, List<UpdateTeamScoreDto> newTeamScores);
    Task<Guid> FinishMatch(Guid matchId);
}