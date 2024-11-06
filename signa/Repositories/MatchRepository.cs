using System.Text.RegularExpressions;
using EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
using signa.Dto.match;
using signa.Entities;
using signa.Interfaces;

namespace signa.Repositories;



public class MatchRepository(ApplicationDbContext context) : Repository<MatchEntity>(context), IMatchRepository;

    /*

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
        return new List<Guid> {match1.Match.Id, match2.Match.Id};
    }
*/