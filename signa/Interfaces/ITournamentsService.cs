using signa.Dto;
using signa.Dto.tournament;

namespace signa.Interfaces;

public interface ITournamentsService
{
    Task<TournamentResponseDto?> GetTournament(Guid tournamentId);
    Task<List<TournamentResponseDto>> GetAllTournaments();
    Task<Guid> CreateTournament(CreateTournamentDto newTournament);
    Task<Guid> UpdateTournament(Guid tournamentId, UpdateTournamentDto updateTournament);
    Task<Guid> DeleteTournament(Guid tournamentId);
}