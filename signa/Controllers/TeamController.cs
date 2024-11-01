using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto;
using signa.Dto.team;
using signa.Interfaces;

namespace signa.Controllers
{
    [ApiController]
    [Route("teams")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamsService teamsService;
        private readonly IUnitOfWork unitOfWork;

        public TeamController(ITeamsService teamsService, IUnitOfWork unitOfWork)
        {
            this.teamsService = teamsService;
            this.unitOfWork = unitOfWork;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto team)
        {
            var teamId = await teamsService.CreateTeam(team);
            await unitOfWork.SaveChangesAsync();
            return Ok(teamId);
        }
        
        [Authorize]
        [HttpGet("{teamId}")]
        public async Task<ActionResult<TeamResponseDto>> Get([FromRoute] Guid teamId)
        {
            var teamResponse = await teamsService.GetTeam(teamId);
            return teamResponse is null ? NotFound() : Ok(teamResponse);
        }

        [Authorize]
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
        [Authorize]
        [HttpDelete("{teamId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid teamId)
        {
            await teamsService.DeleteTeam(teamId);
            await unitOfWork.SaveChangesAsync();
            return Ok(teamId);
        }
    }
}