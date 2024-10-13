using Microsoft.AspNetCore.Mvc;
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
        
        [HttpPost("create-matches")]
        public async Task<IActionResult> Create([FromBody] Guid tournamentId)
        {
            var matchesId = await matchesService.CreateAllMatches(tournamentId);
            return Ok(matchesId);
        }
    }
}