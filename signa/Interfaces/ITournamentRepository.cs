using signa.Entities;

namespace signa.Interfaces;

public interface ITournamentRepository
{
    Task<Guid> Create(TournamentEntity tournamentEntity);
    Task<TournamentEntity?> Get(Guid userId);
    Task<List<TournamentEntity>> GetAll();
    Task<Guid> Update(TournamentEntity newTournamentEntity);
}