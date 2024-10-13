namespace signa.Interfaces;

public interface IMatchesService
{
    Task<List<Guid>> CreateAllMatches(Guid tournamentId);
}
