using signa.Dto.team;
using signa.Entities;

namespace signa.Interfaces.Services;

public interface ITeamsService
{
    Task<TeamResponseDto?> GetTeam(Guid teamId);
    Task<TeamEntity> GetTeamEntity(Guid teamId);
    Task<TeamEntity> GetTeamEntityByCaptainId(Guid captainId);
    Task<List<TeamResponseDto>> GetTeamsByTournamentId(Guid tournamentId);
    Task<Guid> CreateTeam(CreateTeamDto newTeam);
    Task<Guid> UpdateTeam(Guid teamId, UpdateTeamDto updateTeam);
    Task<Guid> DeleteTeam(Guid teamId);
}