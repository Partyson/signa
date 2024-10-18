using Mapster;
using signa.Dto;
using signa.Dto.team;
using signa.Dto.user;
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

    public async Task<Guid> CreateTeam(CreateTeamDto newTeam)
    {
        var id = await teamRepository.Create(newTeam);
        logger.LogInformation($"Team {id} created");
        
        return id;
    }

    public async Task<Guid> UpdateTeam(Guid teamId, UpdateTeamDto updateTeam)
    {
        var newTeamEntity = updateTeam.Adapt<TeamEntity>();
        newTeamEntity.Id = teamId;
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