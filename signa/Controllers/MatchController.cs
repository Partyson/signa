using EntityFrameworkCore.UnitOfWork.Interfaces;
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
        private readonly IUnitOfWork unitOfWork;


        public MatchController(IMatchesService matchesService, IUnitOfWork unitOfWork)
        {
            this.matchesService = matchesService;
            this.unitOfWork = unitOfWork;
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateForTournament([FromQuery] Guid tournamentId)
        {
            var matchesId = await matchesService.CreateMatchesForTournament(tournamentId);
            await unitOfWork.SaveChangesAsync();
            return Ok(matchesId);
        }

        [HttpGet]
        public async Task<ActionResult<List<MatchResponseDto>>> GetMatchesByTournamentId([FromQuery] Guid tournamentId)
        {
            var matches = await matchesService.GetMatchesByTournamentId(tournamentId);
            return Ok(matches);
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateMatchResult([FromQuery] Guid matchId,
            [FromBody] UpdateMatchResultDto updateMatchResultDto)
        {
            var updatedMatchId = await matchesService.UpdateResult(matchId, updateMatchResultDto);
            unitOfWork.SaveChanges();
            return Ok(updatedMatchId);
        }

        [HttpPatch]
        public async Task<ActionResult> SwapTeams([FromBody] SwapTeamDto swapTeams)
        {
            var matchWithSwappedTeamsId = await matchesService.SwapTeams(swapTeams.matchTeam1, swapTeams.matchTeam2);
            return Ok(matchWithSwappedTeamsId);
        }

        [HttpPost("{matchId}")]
        public async Task<ActionResult> FinishMatch([FromRoute] Guid matchId)
        {
            var nextMatchId = await matchesService.FinishMatch(matchId);
            return Ok(nextMatchId);
        }
    }
}