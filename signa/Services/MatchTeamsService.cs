using LinqKit;
using Microsoft.EntityFrameworkCore;
using signa.Dto.match;
using signa.Dto.team;
using signa.Interfaces;

namespace signa.Services;

public class MatchTeamsService : IMatchTeamsService
{
    private readonly IMatchTeamRepository matchTeamRepository;

    public MatchTeamsService(IMatchTeamRepository matchTeamRepository)
    {
        this.matchTeamRepository = matchTeamRepository;
    }

    public async Task<Guid> UpdateResult(Guid matchId, List<UpdateTeamScoreDto> newTeamScores)
    {
        var query = matchTeamRepository.SearchMatchById(matchId);
        var matchTeamEntities = await matchTeamRepository.SearchAsync(query);
        for (var i = 0; i < 2; i++)
        {
            matchTeamEntities[i].Score = newTeamScores[0].Id == matchTeamEntities[i].Team.Id ?
                newTeamScores[0].Score :
                newTeamScores[1].Score;
            matchTeamEntities[i].Match.UpdatedAt = DateTime.Now;
        }
 
        return matchTeamEntities[0].Match.Id;
    }

    public async Task<Guid> FinishMatch(Guid matchId)
    {
        var query = matchTeamRepository.SearchMatchById(matchId);
        var matchTeamsEntity = await matchTeamRepository.SearchAsync(query);
        var winner = matchTeamsEntity.MaxBy(x => x.Score).Team;
        var nextMatch = matchTeamsEntity.First().Match.NextMatch;
        nextMatch.Teams.Add(winner);
        return nextMatch.Id;
    }
}