using Microsoft.AspNetCore.Mvc;
using signa.Dto;
using signa.Interfaces;

namespace signa.Controllers
{
    [ApiController]
    [Route("{tournamentId}")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamsService teamsService;

        public TeamController(ITeamsService teamsService)
        {
            this.teamsService = teamsService;
        }
        
        [HttpPost("create-team")]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto team)
        {
            var teamId = await teamsService.CreateTeam(team);
            return Ok(teamId);
        }
        
        [HttpGet("{teamId}")]
        public async Task<ActionResult<TeamResponseDto>> Get([FromRoute] Guid teamId)
        {
            var teamResponse = await teamsService.GetTeam(teamId);
            return teamResponse is null ? NotFound() : Ok(teamResponse);
        }

        [HttpGet("teams")]
        public async Task<ActionResult<List<TeamResponseDto>>> GetAll()
        {
            var teams = await teamsService.GetAllTeams();
            return Ok(teams);
        }

        [HttpPatch("{teamId}")]
        public async Task<IActionResult> Update(Guid teamId, [FromBody] UpdateTeamDto team)
        {
            var updatedTeamId = await teamsService.UpdateTeam(teamId, team);
            return Ok(updatedTeamId);
        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid teamId)
        {
            await teamsService.DeleteTeam(teamId);
            return Ok(teamId);
        }
    }
}