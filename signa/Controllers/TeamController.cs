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
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto team)
        {
            var validationResult = await validator.ValidateAsync(team);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToString(Environment.NewLine));
            var teamId = await teamsService.CreateTeam(team);
            if (teamId.IsError)
                return Problem(teamId.FirstError.Description, statusCode: teamId.FirstError.Type.ToStatusCode());
            await unitOfWork.SaveChangesAsync();
            return Ok(teamId.Value);
        }
        
        [HttpGet("{teamId}")]
        public async Task<ActionResult<TeamResponseDto>> Get([FromRoute] Guid teamId)
        {
            var teamResponse = await teamsService.GetTeam(teamId);

            if (teamResponse.IsError)
                return Problem(teamResponse.FirstError.Description,
                    statusCode: teamResponse.FirstError.Type.ToStatusCode());
            
            return Ok(teamResponse.Value);
        }

        [HttpGet("get-teams")]
        public async Task<ActionResult<List<TeamResponseDto>>> GetTeamsByTournamentId([FromQuery] Guid tournamentId)
        {
            var teams = await teamsService.GetTeamsByTournamentId(tournamentId);
            
            if (teams.IsError)
                return Problem(teams.FirstError.Description,
                    statusCode: teams.FirstError.Type.ToStatusCode());
            
            return Ok(teams.Value);
        }

        [Authorize(Roles = "Admin,Organizer")]
        [HttpPatch("{teamId}/update")]
        public async Task<IActionResult> Update(Guid teamId, [FromBody] UpdateTeamDto team)
        {
            var updatedTeamId = await teamsService.UpdateTeam(teamId, team);
            
            if (updatedTeamId.IsError)
                return Problem(updatedTeamId.FirstError.Description,
                    statusCode: updatedTeamId.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(updatedTeamId.Value);
        }
        
        [Authorize(Roles = "Admin,Organizer")]
        [HttpDelete("{teamId}/delete")]
        public async Task<IActionResult> Delete([FromRoute] Guid teamId)
        {
            var deletedTeamId = await teamsService.DeleteTeam(teamId);
            
            if (deletedTeamId.IsError)
                return Problem(deletedTeamId.FirstError.Description,
                    statusCode: deletedTeamId.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(deletedTeamId.Value);
        }
    }
}