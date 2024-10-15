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
}