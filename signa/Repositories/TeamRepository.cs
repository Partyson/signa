using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
using signa.Dto.team;
using signa.Entities;
using signa.Interfaces;

namespace signa.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext context;

    public TeamRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Guid> Create(CreateTeamDto newTeam)
    {
        var teamEntity = new TeamEntity();
        teamEntity.Id = Guid.NewGuid();
        teamEntity.CreatedAt = DateTime.Now;
        teamEntity.Tournament = await context.Tournaments.Include(tournamentEntity => tournamentEntity.Teams)
            .FirstOrDefaultAsync(t => newTeam.TournamentId == t.Id);
        teamEntity.Tournament.Teams.Add(teamEntity);
        
        foreach (var membersId in newTeam.MembersId)
            teamEntity.Members = teamEntity.Members.Append(await context.Users.FirstOrDefaultAsync(u => u.Id == membersId)).ToList();
        
        teamEntity.Captain = await context.Users.FirstOrDefaultAsync(u => u.Id == newTeam.CaptainId);
        teamEntity.Title = newTeam.Title;
        await context.Teams.AddAsync(teamEntity);
        await context.SaveChangesAsync();
        return teamEntity.Id;
    }

    public async Task<TeamEntity?> Get(Guid teamId)
    {
        var team = await Task.FromResult(context.Teams
            .AsNoTracking()
            .Include(t => t.Members)
            .Include(t => t.Captain)
            .FirstOrDefault(t => t.Id == teamId));
        return team;
    }

    public async Task<List<TeamEntity>> GetAll()
    {
        return await context.Teams
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Guid> Update(TeamEntity newTeamEntity)
    {
        await context.Teams
            .Where(t => t.Id == newTeamEntity.Id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(t => t.Title, newTeamEntity.Title)
                    .SetProperty(t => t.Captain, newTeamEntity.Captain)
                    .SetProperty(t => t.Members, newTeamEntity.Members));
        await context.SaveChangesAsync();
        return newTeamEntity.Id;
    }

    public async Task<Guid> Delete(Guid teamId)
    {
        await context.Teams
            .Where(t => t.Id == teamId)
            .ExecuteDeleteAsync();
        return teamId;
    }
}