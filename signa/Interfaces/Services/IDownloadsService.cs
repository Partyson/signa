namespace signa.Interfaces.Services;

public interface IDownloadsService
{
    Task<byte[]> DownloadTournamentPlayers(Guid tournamentId);
}