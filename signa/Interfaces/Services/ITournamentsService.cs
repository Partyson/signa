using ErrorOr;
using signa.Dto.tournament;
using signa.Entities;

namespace signa.Interfaces.Services;

public interface ITournamentsService
{
    Task<TournamentInfoDto?> GetTournamentResponse(Guid tournamentId);
    Task<ErrorOr<TournamentEntity>> GetTournament(Guid tournamentId);
    Task<List<TournamentListItemDto>> GetAllTournaments();
    Task<Guid> CreateTournament(CreateTournamentDto newTournament);
    Task<Guid> UpdateTournament(Guid tournamentId, UpdateTournamentDto updateTournament);
    Task<Guid> DeleteTournament(Guid tournamentId);
}