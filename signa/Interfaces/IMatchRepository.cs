namespace signa.Interfaces;

public interface IMatchRepository
{
    Task<List<Guid>> CreateMatches(Guid tournamentId);
}