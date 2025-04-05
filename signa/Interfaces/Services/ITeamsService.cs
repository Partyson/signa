using ErrorOr;
using signa.Dto.team;
using signa.Entities;

namespace signa.Interfaces.Services;

public interface ITeamsService
{
    Task<ErrorOr<TeamResponseDto?>> GetTeam(Guid teamId);
    Task<ErrorOr<TeamEntity>> GetTeamEntity(Guid teamId);
    Task<ErrorOr<List<TeamEntity>>> GetTeamEntitiesByIds(List<Guid> teamIds);
    Task<ErrorOr<TeamEntity>> GetTeamEntityByCaptainId(Guid captainId);
    Task<ErrorOr<List<TeamResponseDto>>> GetTeamsByTournamentId(Guid tournamentId);
    Task<ErrorOr<Guid>> CreateTeam(CreateTeamDto newTeam);
    Task<ErrorOr<Guid>> UpdateTeam(Guid teamId, UpdateTeamDto updateTeam);
    Task<ErrorOr<Guid>> DeleteTeam(Guid teamId);
}