using Microsoft.AspNetCore.Mvc;
using signa.Dto;
using signa.Interfaces;

namespace signa.Controllers
{
    [ApiController]
    [Route("tournament")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentsService tournamentsService;

        public TournamentController(ITournamentsService tournamentsService)
        {
            this.tournamentsService = tournamentsService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTournamentDto tournament)
        {
            var tournamentId = await tournamentsService.CreateTournament(tournament);
            return Ok(tournamentId);
        }
        
        [HttpGet("{tournamentId}")]
        public async Task<ActionResult<TournamentResponseDto>> Get([FromRoute] Guid tournamentId)
        {
            var tournamentResponse = await tournamentsService.GetTournament(tournamentId);
            return tournamentResponse is null ? NotFound() : Ok(tournamentResponse);
        }

        [HttpGet]
        public async Task<ActionResult<List<TournamentResponseDto>>> GetAll()
        {
            var tournaments = await tournamentsService.GetAllTournaments();
            return Ok(tournaments);
        }

        [HttpPatch("{tournamentId}")]
        public async Task<IActionResult> UpdateUser(Guid tournamentId, [FromBody] UpdateTournamentDto tournament)
        {
            var updatedTournamentId = await tournamentsService.UpdateTournament(tournamentId, tournament);
            return Ok(updatedTournamentId);
        }

        [HttpDelete("{tournamentId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid tournamentId)
        {
            await tournamentsService.DeleteTournament(tournamentId);
            return Ok(tournamentId);
        }
    }
}