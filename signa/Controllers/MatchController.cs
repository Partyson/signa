using Microsoft.AspNetCore.Mvc;
using signa.Dto.match;
using signa.Interfaces;

namespace signa.Controllers
{
    [ApiController]
    [Route("matches")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchesService matchesService;

        public MatchController(IMatchesService matchesService)
        {
            this.matchesService = matchesService;
        }
        
        [HttpPost("{tournamentId}")]
        public async Task<ActionResult> CreateForTournament([FromRoute] Guid tournamentId)
        {
            var matchesId = await matchesService.CreateMatchesForTournament(tournamentId);
            return Ok(matchesId);
        }

        [HttpPatch("{matchId}")]
        public async Task<ActionResult> UpdateMatchResult([FromRoute] Guid matchId,
            [FromBody] UpdateMatchResultDto updateMatchResultDto)
        {
            var updatedMatchId = await matchesService.UpdateResult(matchId, updateMatchResultDto);
            return Ok(updatedMatchId);
        }
    }
}