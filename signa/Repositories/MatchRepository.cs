using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
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

    public async Task<List<Guid>> CreateMatches(Guid tournamentId)
    {
        var matches = new List<MatchEntity>();
        var tournament = await context.Tournaments.Include(x => x.Teams)
            .FirstOrDefaultAsync(t => tournamentId == t.Id);
        var matchCount = 15;//tournament.Teams.Count - 1;
        for (var i = 0; i < matchCount; i++)
        {
            var match = new MatchEntity();
            var matchId = Guid.NewGuid();
            match.Id = matchId;
            match.Tournament = tournament;
            match.CreatedAt = DateTime.Now;
            match.UpdatedAt = DateTime.Now;
            matches.Add(match);
        }

        ConnectMatches(matches);
        
        foreach (var match in matches)
            await context.AddAsync(match);
        
        await context.SaveChangesAsync();
        return matches.Select(x => x.Id).ToList();
    }

    private void ConnectMatches(List<MatchEntity> matches)
    {
        var roundCount = (int)Math.Log2(matches.Count + 1);
        var currentMatchIndex = 0;
        while (roundCount > 1)
        {
            var step = (int)Math.Pow(2, roundCount - 1);
            var matchInRound = step;
            for (var i = 0; i < matchInRound / 2; ++i)
            {
                matches[currentMatchIndex].NextMatch = matches[currentMatchIndex + step];
                matches[currentMatchIndex + 1].NextMatch = matches[currentMatchIndex + step];
                currentMatchIndex += 2;
                step--;
            }

            roundCount--;
        }
    }

    public async Task<List<MatchEntity>> GetMatches(Guid tournamentId)
    {
        var tournament = await context.Tournaments.Include(x => x.Matches)
            .FirstOrDefaultAsync(t => t.Id == tournamentId);
        return tournament.Matches;
    }
}