using signa.Dto;
using signa.Dto.match;
using signa.Dto.team;
using signa.Dto.tournament;

namespace signa.Interfaces;

public interface ITournamentsService
{
    Task<TournamentInfoDto?> GetTournament(Guid tournamentId);
    Task<List<TournamentListItemDto>> GetAllTournaments();
    Task<List<MatchResponseDto>> GetMatches(Guid tournamentId);
    Task<List<TeamResponseDto>> GetTeams(Guid tournamentId);
    Task<Guid> CreateTournament(CreateTournamentDto newTournament);
    Task<Guid> UpdateTournament(Guid tournamentId, UpdateTournamentDto updateTournament);
    Task<Guid> DeleteTournament(Guid tournamentId);
}