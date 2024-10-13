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

    public async Task<List<Guid>> CreateMatches(List<MatchEntity> matches)
    {
        foreach (var match in matches)
            await context.AddAsync(match);
        await context.SaveChangesAsync();
        return matches.Select(x => x.Id).ToList();
    }

    public async Task<List<MatchEntity>> GetMatches(TournamentEntity tournament)
    {
        return tournament.Matches;
    }
}