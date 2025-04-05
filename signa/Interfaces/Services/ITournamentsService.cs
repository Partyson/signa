using ErrorOr;
using signa.Dto.tournament;
using signa.Entities;

namespace signa.Interfaces.Services;

public interface ITournamentsService
{
    Task<ErrorOr<TournamentInfoDto?>> GetTournamentResponse(Guid tournamentId);
    Task<ErrorOr<TournamentEntity>> GetTournament(Guid tournamentId);
    Task<List<TournamentListItemDto>> GetAllTournaments();
    Task<ErrorOr<Guid>> CreateTournament(CreateTournamentDto newTournament);
    Task<ErrorOr<Guid>> UpdateTournament(Guid tournamentId, UpdateTournamentDto updateTournament);
    Task<ErrorOr<Guid>> DeleteTournament(Guid tournamentId);
}