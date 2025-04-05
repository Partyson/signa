using signa.Dto.team;
using signa.Extensions;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;
using ErrorOr;

namespace signa.Services;

public class MatchTeamsService : IMatchTeamsService
{
    private readonly IMatchTeamRepository matchTeamRepository;

    public MatchTeamsService(IMatchTeamRepository matchTeamRepository)
    {
        this.matchTeamRepository = matchTeamRepository;
    }

    public async Task<ErrorOr<Guid>> UpdateResult(Guid matchId, List<UpdateTeamScoreDto> newTeamScores)
    {
        var query = matchTeamRepository.SearchMatchByIdQuery(matchId);
        var matchTeamEntities = await matchTeamRepository.SearchAsync(query);

        if (matchTeamEntities.Count == 0)
            return Error.NotFound("General.NotFound", $"Can't find matchTeamEntities by id {matchId}");
        
        for (var i = 0; i < 2; i++)
        {
            matchTeamEntities[i].Score = newTeamScores[0].Id == matchTeamEntities[i].Team.Id 
                ? newTeamScores[0].Score 
                : newTeamScores[1].Score;
            matchTeamEntities[i].Match.UpdatedAt = DateTime.Now;
        }
 
        return matchTeamEntities[0].Match.Id;
    }

    public async Task<ErrorOr<Guid>> FinishMatch(Guid matchId)
    {
        var query = matchTeamRepository.SearchMatchByIdQuery(matchId);
        var matchTeamsEntity = await matchTeamRepository.SearchAsync(query);

        if (matchTeamsEntity.Count == 0)
            return Error.NotFound("General.NotFound", $"Can't find matchTeamEntities by id {matchId}");
        
        var winner = matchTeamsEntity.MaxBy(x => x.Score).Team;
        var nextMatch = matchTeamsEntity.First().Match.NextMatch;
        nextMatch.Teams.Add(winner);
        return nextMatch.Id;
    }
}