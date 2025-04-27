using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using signa.Dto.match;
using signa.Extensions;
using signa.Interfaces.Services;

namespace signa.Controllers
{
    [ApiController]
    [Route("matches")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchesService matchesService;
        private readonly IUnitOfWork unitOfWork;


        public MatchController(IMatchesService matchesService, IUnitOfWork unitOfWork)
        {
            this.matchesService = matchesService;
            this.unitOfWork = unitOfWork;
        }
        
        [Authorize(Roles = "Admin,Organizer")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateForTournament([FromQuery] Guid tournamentId)
        {
            var matchesId = await matchesService.CreateMatchesForTournament(tournamentId);
            
            if (matchesId.IsError)
                return Problem(matchesId.FirstError.Description,
                    statusCode: matchesId.FirstError.Type.ToStatusCode());
            
            await unitOfWork.SaveChangesAsync();
            return Ok(matchesId.Value);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<MatchResponseDto>>> GetMatchesByTournamentId([FromQuery] Guid tournamentId)
        {
            var matches = await matchesService.GetMatchesByTournamentId(tournamentId);
            
            if (matches.IsError)
                return Problem(matches.FirstError.Description,
                    statusCode: matches.FirstError.Type.ToStatusCode());
            
            return Ok(matches.Value);
        }

        [Authorize(Roles = "Admin,Organizer")]
        [HttpPatch("update")]
        public async Task<ActionResult> UpdateMatchResult([FromQuery] Guid matchId,
            [FromBody] UpdateMatchResultDto updateMatchResultDto)
        {
            var updatedMatchId = await matchesService.UpdateResult(matchId, updateMatchResultDto);
            
            if (updatedMatchId.IsError)
                return Problem(updatedMatchId.FirstError.Description,
                    statusCode: updatedMatchId.FirstError.Type.ToStatusCode());
            
            unitOfWork.SaveChanges();
            return Ok(updatedMatchId.Value);
        }

        [Authorize(Roles = "Admin,Organizer")]
        [HttpPatch("swap-teams")]
        public async Task<ActionResult> SwapTeams([FromBody] SwapTeamDto swapTeams)
        {
            var matchWithSwappedTeamsId = await matchesService.SwapTeams(swapTeams.matchTeam1, swapTeams.matchTeam2);

            if (matchWithSwappedTeamsId.IsError)
                return Problem(matchWithSwappedTeamsId.FirstError.Description,
                    statusCode: matchWithSwappedTeamsId.FirstError.Type.ToStatusCode());
            
            return Ok(matchWithSwappedTeamsId.Value);
        }

        [Authorize(Roles = "Admin,Organizer")]
        [HttpPost("{matchId}/finish")]
        public async Task<ActionResult> FinishMatch([FromRoute] Guid matchId)
        {
            var nextMatchId = await matchesService.FinishMatch(matchId);
            
            if (nextMatchId.IsError)
                return Problem(nextMatchId.FirstError.Description,
                    statusCode: nextMatchId.FirstError.Type.ToStatusCode());
            
            return Ok(nextMatchId.Value);
        }
    }
}