using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
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

    public async Task<Guid> Create(TeamEntity teamEntity)
    {
        await context.Teams.AddAsync(teamEntity);
        await context.SaveChangesAsync();
        return teamEntity.Id;
    }

    public async Task<TeamEntity?> Get(Guid teamId)
    {
        return await Task.FromResult(context.Teams
            .AsNoTracking()
            .FirstOrDefault(t => t.Id == teamId));
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