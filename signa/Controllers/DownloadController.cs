using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Extensions;
using signa.Interfaces.Services;

namespace signa.Controllers;

public class DownloadController : ControllerBase
{
    private readonly IDownloadsService downloadsService;

    public DownloadController(IDownloadsService downloadsService)
    {
        this.downloadsService = downloadsService;
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpGet]
    [Route("download/user/{tournamentId}")]
    public async Task<IActionResult> DownloadPlayersByTournament(Guid tournamentId)
    {
        var docxContent = await downloadsService.DownloadTournamentPlayers(tournamentId);
        if (docxContent.IsError)
            return Problem(docxContent.FirstError.Description,
                statusCode: docxContent.FirstError.Type.ToStatusCode());
        return File(docxContent.Value,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "Participants.docx");
    }
}