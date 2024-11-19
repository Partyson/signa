using Mapster;
using Microsoft.EntityFrameworkCore;
using signa.Dto.team;
using signa.Entities;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;

namespace signa.Services;

public class TeamsService : ITeamsService
{
    private readonly ITeamRepository teamRepository;
    private readonly ILogger<TeamsService> logger;
    private readonly ITournamentsService tournamentsService;
    private readonly IUsersService usersService;

    public TeamsService(ITeamRepository teamRepository, ILogger<TeamsService> logger,
        ITournamentsService tournamentsService, IUsersService usersService)
    {
        this.teamRepository = teamRepository;
        this.logger = logger;
        this.tournamentsService = tournamentsService;
        this.usersService = usersService;
    }

    public async Task<TeamResponseDto?> GetTeam(Guid teamId)
    {
        var query = teamRepository.SingleResultQuery()
            .Include(x => 
                x.Include(y => y.Members).Include(y => y.Captain))
            .AndFilter(x => x.Id == teamId);
        var teamEntity = await teamRepository.FirstOrDefaultAsync(query);
        if (teamEntity == null)
        {
            logger.LogWarning("Team not found from database");
            return null;
        }

        logger.LogInformation($"Team {teamEntity.Id} is retrieved from database");
        return teamEntity.Adapt<TeamResponseDto>();
    }

    public async Task<List<TeamResponseDto>> GetTeamsByTournamentId(Guid tournamentId)
    {
        var query = teamRepository.MultipleResultQuery()
            .Include(x => 
                x.Include(y => y.Tournament)
                    .Include(y => y.Members))
            .AndFilter(x => x.Tournament.Id == tournamentId);
        var teamEntities = await teamRepository.SearchAsync(query);
        if (teamEntities.Count == 0)
            logger.LogWarning("Tournament don't have any teams");
        logger.LogInformation($"Tournament {tournamentId} has {teamEntities.Count} teams");
        var teams = teamEntities.Select(x => x.Adapt<TeamResponseDto>());
        return teams.ToList();
    }

    public async Task<Guid> CreateTeam(CreateTeamDto newTeam)
    {
        var members = await usersService.GetUserEntitiesByIds(newTeam.MembersId);
        if (members.Count < newTeam.MembersId.Count)
            throw new Exception("Sosal?");
        var capitan = members.First(x => x.Id == newTeam.CaptainId);
        var tournament = await tournamentsService.GetTournament(newTeam.TournamentId);
        if (tournament == null)
            throw new Exception("Tournament not found");

        var newTeamEntity = new TeamEntity
        {
            Title = newTeam.Title,
            Members = members,
            Captain = capitan,
            Tournament = tournament
        };
        var addedTeamEntity = await teamRepository.AddAsync(newTeamEntity);
        return addedTeamEntity.Id;
    }

    public async Task<Guid> UpdateTeam(Guid teamId, UpdateTeamDto updateTeam)
    {
        var query = teamRepository.SingleResultQuery().AndFilter(x => x.Id == teamId);
        var teamEntity = await teamRepository.FirstOrDefaultAsync(query);
        updateTeam.Adapt(teamEntity);
        teamEntity.UpdatedAt = DateTime.Now;
        logger.LogInformation($"Team {teamId} updated");
        return teamId;
    }

    public async Task<Guid> DeleteTeam(Guid teamId)
    {
        var query = teamRepository.SingleResultQuery().AndFilter(x => x.Id == teamId);
        var teamEntity = await teamRepository.FirstOrDefaultAsync(query);
        teamRepository.Remove(teamEntity);
        logger.LogInformation($"Team {teamId} deleted");
        
        return teamId;
    }
}

