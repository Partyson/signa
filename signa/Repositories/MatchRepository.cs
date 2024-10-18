using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
using signa.Dto.match;
using signa.Entities;
using signa.Interfaces;

namespace signa.Repositories;



public class MatchRepository : IMatchRepository
{
    private readonly ApplicationDbContext context;

    public MatchRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Guid>> CreateMatches(List<MatchEntity> matches)
    {
        foreach (var match in matches)
        {
            match.Tournament.Matches.Add(match);
            await context.Matches.AddAsync(match);
        }
        await context.SaveChangesAsync();
        return matches.Select(x => x.Id).ToList();
    }

    public async Task<Guid> UpdateResults(Guid matchId, UpdateMatchResultDto updateMatchResultDto)
    {
        var teams = updateMatchResultDto.Teams;
        var matchResults = context.MatchTeams
            .Include(mt => mt.Match)
            .Include(mt => mt.Team)
            .Where(mt => mt.Match.Id == matchId);
        await matchResults
            .Where(mt => mt.Team.Id == teams[0].Id)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(mt => mt.Score, teams[0].Score));
        await matchResults
            .Where(mt => mt.Team.Id == teams[1].Id)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(mt => mt.Score, teams[1].Score));
         return matchId;   
    }

    public async Task<List<Guid>> SwapTeams(Guid tournamentId, MatchTeamDto matchTeam1, MatchTeamDto matchTeam2)
    {
        var matches = await context.MatchTeams
            .Include(mt => mt.Match)
            .ThenInclude(m => m.Tournament)
            .Include(mt => mt.Team)
            .ToListAsync();
        var match1Query = matches.Where(mt => mt.Match.Id == matchTeam1.MatchId && mt.Team.Id == matchTeam1.TeamId);
        var match1 = match1Query.FirstOrDefault();
        await context.MatchTeams.Where(mt => mt.Match.Id == matchTeam1.MatchId && mt.Team.Id == matchTeam1.TeamId).ExecuteDeleteAsync();
        var match2Query = matches.Where(mt => mt.Match.Id == matchTeam2.MatchId && mt.Team.Id == matchTeam2.TeamId);
        var match2 = match2Query.FirstOrDefault();
        await context.MatchTeams.Where(mt => mt.Match.Id == matchTeam2.MatchId && mt.Team.Id == matchTeam2.TeamId).ExecuteDeleteAsync();
        (match1.Team, match2.Team) = (match2.Team, match1.Team);
        
        await context.MatchTeams.AddAsync(match1);
        await context.MatchTeams.AddAsync(match2);
        await context.SaveChangesAsync();
        return new List<Guid>() {match1.Match.Id, match2.Match.Id};
    }

    public async Task<Guid> FinishMatch(Guid matchId)
    {
        var matches = await context.MatchTeams
            .Include(mt => mt.Match)
            .ThenInclude(matchEntity => matchEntity.NextMatch)
            .Include(mt => mt.Team)
            .ToListAsync();
       var currentMatch = matches.Where(mt => mt.Match.Id == matchId).ToList();
       var winner = currentMatch.MaxBy(mt => mt.Score).Team;
       var nextMatch = currentMatch.First().Match.NextMatch;
       nextMatch.Teams.Add(winner);
       winner.Matches.Add(nextMatch);
       await context.SaveChangesAsync();
       return nextMatch.Id;
    }
}