using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.tournament;
using signa.Extensions;
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
            return Ok(tournamentId.Value);
        }
        
        [HttpGet("{tournamentId}")]
        public async Task<ActionResult<TournamentInfoDto>> Get([FromRoute] Guid tournamentId)
        {
            var tournamentResponse = await tournamentsService.GetTournamentResponse(tournamentId);

            if (tournamentResponse.IsError)
                return Problem(tournamentResponse.FirstError.Description,
                    statusCode: tournamentResponse.FirstError.Type.ToStatusCode());
            
            return Ok(tournamentResponse.Value);
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
            
            if (updatedTournamentId.IsError)
                return Problem(updatedTournamentId.FirstError.Description,
                    statusCode: updatedTournamentId.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(updatedTournamentId.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{tournamentId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid tournamentId)
        {
            var deletedTournamentId = await tournamentsService.DeleteTournament(tournamentId);
            
            if (deletedTournamentId.IsError)
                return Problem(deletedTournamentId.FirstError.Description,
                    statusCode: deletedTournamentId.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(deletedTournamentId.Value);
        }
    }
}