using Mapster;
using signa.Dto;
using signa.Entities;
using signa.Interfaces;

namespace signa.Services;

public class TeamsService : ITeamsService
{
    private readonly ITeamRepository teamRepository;
    private readonly ILogger<TeamsService> logger;

    public TeamsService(ITeamRepository teamRepository, ILogger<TeamsService> logger)
    {
        this.teamRepository = teamRepository;
        this.logger = logger;
    }

    public async Task<TeamResponseDto?> GetTeam(Guid teamId)
    {
        var teamEntity = await teamRepository.Get(teamId);
        if (teamEntity != null)
        {
            logger.LogInformation($"Team {teamId} is retrieved from database");
            return teamEntity.Adapt<TeamResponseDto>();
        }
        
        logger.LogWarning($"Team {teamId} not found from database");
        return null;
    }

    public async Task<List<TeamResponseDto>> GetAllTeams()
    {
        var teams = await teamRepository.GetAll();
        logger.LogInformation($"Teams: {teams.Count}");
        
        return teams.Adapt<List<TeamResponseDto>>();
    }

    public async Task<Guid> CreateTeam(CreateTeamDto newTeam)
    {
        var teamEntity = newTeam.Adapt<TeamEntity>();
        teamEntity.Id = Guid.NewGuid();
        teamEntity.CreatedAt = DateTime.Now;
        teamEntity.UpdatedAt = teamEntity.CreatedAt;
        var id = await teamRepository.Create(teamEntity);
        logger.LogInformation($"Team {teamEntity.Id} created");
        
        return id;
    }

    public async Task<Guid> UpdateTeam(Guid teamId, UpdateTeamDto updateTeam)
    {
        var newTeamEntity = updateTeam.Adapt<TeamEntity>();
        newTeamEntity.Id = Guid.NewGuid();
        var updatedTeamId = await teamRepository.Update(newTeamEntity);
        logger.LogInformation($"Team {updatedTeamId} updated");
        
        return updatedTeamId;
    }

    public async Task<Guid> DeleteTeam(Guid teamId)
    {
        var id = await teamRepository.Delete(teamId);
        logger.LogInformation($"Team {id} deleted");
        
        return id;
    }
}