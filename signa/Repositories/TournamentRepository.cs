using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
using signa.Entities;
using signa.Interfaces;

namespace signa.Repositories;

public class TournamentRepository : ITournamentRepository
{
    private readonly ApplicationDbContext context;

    public TournamentRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Guid> Create(TournamentEntity tournamentEntity)
    {
        await context.Tournaments.AddAsync(tournamentEntity);
        await context.SaveChangesAsync();
        return tournamentEntity.Id;
    }

    public async Task<TournamentEntity?> Get(Guid tournamentId)
    {
        return await Task.FromResult(context.Tournaments
            .AsNoTracking()
            .FirstOrDefault(t => t.Id == tournamentId));
    }

    public async Task<List<TournamentEntity>> GetAll()
    {
        var tournamentsEntity = await context.Tournaments
            .AsNoTracking()
            .ToListAsync();
        return tournamentsEntity;
    }

    public async Task<Guid> Update(TournamentEntity newTournamentEntity)
    {
        await context.Tournaments
            .Where(t => t.Id == newTournamentEntity.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.Title, newTournamentEntity.Title)
                .SetProperty(b => b.Location, newTournamentEntity.Location)
                .SetProperty(b => b.SportType, newTournamentEntity.SportType)
                .SetProperty(b => b.TeamsMembersMaxNumber, newTournamentEntity.TeamsMembersMaxNumber)
                .SetProperty(b => b.Gender, newTournamentEntity.Gender)
                .SetProperty(b => b.MinFemaleCount, newTournamentEntity.MinFemaleCount)
                .SetProperty(b => b.MinMaleCount, newTournamentEntity.MinMaleCount)
                .SetProperty(b => b.MaxTeamsCount, newTournamentEntity.MaxTeamsCount)
                .SetProperty(b => b.StartedAt, newTournamentEntity.StartedAt)
                .SetProperty(b => b.EndRegistrationAt, newTournamentEntity.EndRegistrationAt)
                .SetProperty(b => b.State, newTournamentEntity.State)
                .SetProperty(b => b.RegulationLink, newTournamentEntity.RegulationLink)
                .SetProperty(b => b.WithGroupStage, newTournamentEntity.WithGroupStage)
                .SetProperty(b => b.UpdatedAt, DateTime.Now));
        await context.SaveChangesAsync();
        return newTournamentEntity.Id;
    }

    public async Task<Guid> Delete(Guid tournamentId)
    {
        await context.Tournaments
            .Where(t => t.Id == tournamentId)
            .ExecuteDeleteAsync();
        return tournamentId;
    }
}