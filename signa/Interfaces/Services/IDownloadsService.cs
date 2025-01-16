using ErrorOr;

namespace signa.Interfaces.Services;

public interface IDownloadsService
{
    Task<ErrorOr<byte[]>> DownloadTournamentPlayers(Guid tournamentId);
}