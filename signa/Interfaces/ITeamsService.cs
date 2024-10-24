using Microsoft.AspNetCore.Mvc;
using signa.Dto;
using signa.Dto.team;
using signa.Entities;

namespace signa.Interfaces;

public interface ITeamsService
{
    Task<TeamResponseDto?> GetTeam(Guid teamId);
    
    Task<List<TeamResponseDto>> GetTeamsByTournamentId(Guid tournamentId);
    Task<Guid> CreateTeam(CreateTeamDto newTeam);
    Task<Guid> UpdateTeam(Guid teamId, UpdateTeamDto updateTeam);
    Task<Guid> DeleteTeam(Guid teamId);
}