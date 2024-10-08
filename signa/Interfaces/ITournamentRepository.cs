using signa.Entities;

namespace signa.Interfaces;

public interface ITournamentRepository
{
    Task<Guid> Create(TournamentEntity tournamentEntity);
    Task<TournamentEntity?> Get(Guid tournamentId);
    Task<List<TournamentEntity>> GetAll();
    Task<Guid> Update(TournamentEntity newTournamentEntity);
    Task<Guid> Delete(Guid tournamentId);
}