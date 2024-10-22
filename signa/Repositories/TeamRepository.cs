using EntityFrameworkCore.Repository;
using signa.DataAccess;
using signa.Dto.team;
using signa.Entities;
using signa.Interfaces;

namespace signa.Repositories;

public class TeamRepository(ApplicationDbContext context) : Repository<TeamEntity>(context), ITeamRepository;
/*{
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
}*/