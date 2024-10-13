﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateForTournament([FromBody] Guid tournamentId)
        {
            var matchesId = await matchesService.CreateMatchesForTournament(tournamentId);
            return Ok(matchesId);
        }

        [HttpGet("{tournamentId}")]
        public async Task<ActionResult<List<MatchResponseDto>>> GetForTournament(Guid tournamentId)
        {
            var matches = await matchesService.GetMatchesForTournament(tournamentId);
            return Ok(matches);
        }
    }
}