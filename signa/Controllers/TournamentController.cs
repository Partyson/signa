using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.tournament;
using signa.Interfaces.Services;
using signa.Validators;

namespace signa.Controllers
{
    [ApiController]
    [Route("tournaments")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentsService tournamentsService;
        private readonly IUnitOfWork unitOfWork;
        private readonly TournamentValidator validator;

        public TournamentController(ITournamentsService tournamentsService, IUnitOfWork unitOfWork, TournamentValidator tournamentValidator)
        {
            this.tournamentsService = tournamentsService;
            this.unitOfWork = unitOfWork;
            validator = tournamentValidator;
        }
        [Authorize(Roles = "Admin,Organizer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTournamentDto tournament)
        {
            var validationResult = await validator.ValidateAsync(tournament);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToString(Environment.NewLine));
            
            var tournamentId = await tournamentsService.CreateTournament(tournament);
            await unitOfWork.SaveChangesAsync();
            return Ok(tournamentId);
        }
        
        [HttpGet("{tournamentId}")]
        public async Task<ActionResult<TournamentInfoDto>> Get([FromRoute] Guid tournamentId)
        {
            var tournamentResponse = await tournamentsService.GetTournamentResponse(tournamentId);
            return tournamentResponse is null ? NotFound() : Ok(tournamentResponse);
        }

        [HttpGet("/tournaments")]
        public async Task<ActionResult<List<TournamentListItemDto>>> GetAll()
        {
            var tournaments = await tournamentsService.GetAllTournaments();
            return Ok(tournaments);
        }

        [Authorize(Roles = "Admin,Organizer")]
        [HttpPatch("{tournamentId}")]
        public async Task<IActionResult> Update(Guid tournamentId, [FromBody] UpdateTournamentDto tournament)
        {
            var updatedTournamentId = await tournamentsService.UpdateTournament(tournamentId, tournament);
            await unitOfWork.SaveChangesAsync();
            return Ok(updatedTournamentId);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{tournamentId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid tournamentId)
        {
            await tournamentsService.DeleteTournament(tournamentId);
            await unitOfWork.SaveChangesAsync();
            return Ok(tournamentId);
        }
    }
}