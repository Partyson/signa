using Microsoft.AspNetCore.Mvc;
using signa.Interfaces.Services;

namespace signa.Controllers;

public class DownloadController : ControllerBase
{
    private readonly IDownloadsService downloadsService;

    public DownloadController(IDownloadsService downloadsService)
    {
        this.downloadsService = downloadsService;
    }

    [HttpGet]
    [Route("download/user/{tournamentId}")]
    public async Task<IActionResult> DownloadPlayersByTournament(Guid tournamentId)
    {
        var docxContent = await downloadsService.DownloadTournamentPlayers(tournamentId);
        return File(docxContent,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "Participants.docx");
    }
}