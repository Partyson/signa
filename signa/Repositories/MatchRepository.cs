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
        var matchesId = new List<Guid>();
        var tournament = await context.Tournaments.Include(x => x.Teams)
            .FirstOrDefaultAsync(t => tournamentId == t.Id);
        var teamsCount = tournament.Teams.Count;
        for (var i = 0; i < teamsCount - 1; i++)
        {
            var match = new MatchEntity();
            var matchId = Guid.NewGuid();
            match.Id = matchId;
            match.Tournament = tournament;
            match.CreatedAt = DateTime.Now;
            match.UpdatedAt = DateTime.Now;
            matchesId.Add(matchId);
            await context.AddAsync(match);
        }
        return matchesId;
    }
}