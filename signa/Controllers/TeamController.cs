using EntityFrameworkCore.UnitOfWork.Interfaces;
using ErrorOr;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.team;
using signa.Extensions;
using signa.Interfaces.Services;
using signa.Validators;

namespace signa.Controllers
{
    [ApiController]
    [Route("teams")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamsService teamsService;
        private readonly IUnitOfWork unitOfWork;
        private readonly TeamValidator validator;

        public TeamController(ITeamsService teamsService, IUnitOfWork unitOfWork, TeamValidator validator)
        {
            this.teamsService = teamsService;
            this.unitOfWork = unitOfWork;
            this.validator = validator;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto team)
        {
            var validationResult = await validator.ValidateAsync(team);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToString(Environment.NewLine));
            var teamId = await teamsService.CreateTeam(team);
            if (teamId.IsError)
                return Problem(teamId.FirstError.Description, statusCode: teamId.FirstError.Type.ToStatusCode());
            await unitOfWork.SaveChangesAsync();
            return Ok(teamId);
        }
        
        [HttpGet("{teamId}")]
        public async Task<ActionResult<TeamResponseDto>> Get([FromRoute] Guid teamId)
        {
            var teamResponse = await teamsService.GetTeam(teamId);
            return teamResponse is null ? NotFound() : Ok(teamResponse);
        }

        [HttpGet]
        public async Task<ActionResult<List<TeamResponseDto>>> GetTeamsByTournamentId([FromQuery] Guid tournamentId)
        {
            var teams = await teamsService.GetTeamsByTournamentId(tournamentId);
            return Ok(teams);
        }

        [Authorize]
        [HttpPatch("{teamId}")]
        public async Task<IActionResult> Update(Guid teamId, [FromBody] UpdateTeamDto team)
        {
            var updatedTeamId = await teamsService.UpdateTeam(teamId, team);
            await unitOfWork.SaveChangesAsync();
            return Ok(updatedTeamId);
        }
        
        [Authorize(Roles = "Admin,Organizer")]
        [HttpDelete("{teamId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid teamId)
        {
            await teamsService.DeleteTeam(teamId);
            await unitOfWork.SaveChangesAsync();
            return Ok(teamId);
        }
    }
}