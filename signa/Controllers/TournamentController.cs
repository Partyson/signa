﻿using EntityFrameworkCore.UnitOfWork.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using signa.Dto;
using signa.Dto.match;
using signa.Dto.team;
using signa.Dto.tournament;
using signa.Interfaces;

namespace signa.Controllers
{
    [ApiController]
    [Route("tournament")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentsService tournamentsService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITeamsService teamsService;
        private readonly IMatchesService matchesService;

        public TournamentController(ITournamentsService tournamentsService, IUnitOfWork unitOfWork, ITeamsService teamsService, IMatchesService matchesService)
        {
            this.tournamentsService = tournamentsService;
            this.unitOfWork = unitOfWork;
            this.teamsService = teamsService;
            this.matchesService = matchesService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTournamentDto tournament)
        {
            var tournamentId = await tournamentsService.CreateTournament(tournament);
            await unitOfWork.SaveChangesAsync();
            return Ok(tournamentId);
        }
        
        [HttpGet("{tournamentId}")]
        public async Task<ActionResult<TournamentInfoDto>> Get([FromRoute] Guid tournamentId)
        {
            var tournamentResponse = await tournamentsService.GetTournamentResponse(tournamentId);
            return tournamentResponse is null ? NotFound() : Ok(tournamentResponse);
        }

        [HttpGet("/tournaments")]
        public async Task<ActionResult<List<TournamentListItemDto>>> GetAll()
        {
            var tournaments = await tournamentsService.GetAllTournaments();
            return Ok(tournaments);
        }
        [HttpGet("{tournamentId}/matches")]
        public async Task<ActionResult<List<MatchResponseDto>>> GetMatches([FromRoute] Guid tournamentId)
        {
            var matches = await matchesService.GetMatchesByTournamentId(tournamentId);
            return Ok(matches);
        }
        [HttpGet("{tournamentId}/teams")]
        public async Task<ActionResult<List<TeamResponseDto>>> GetTeams([FromRoute] Guid tournamentId)
        {
            var teams = await teamsService.GetTeamsByTournamentId(tournamentId);
            return Ok(teams);
        }

        [HttpPatch("{tournamentId}")]
        public async Task<IActionResult> Update(Guid tournamentId, [FromBody] UpdateTournamentDto tournament)
        {
            var updatedTournamentId = await tournamentsService.UpdateTournament(tournamentId, tournament);
            await unitOfWork.SaveChangesAsync();
            return Ok(updatedTournamentId);
        }

        [HttpDelete("{tournamentId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid tournamentId)
        {
            await tournamentsService.DeleteTournament(tournamentId);
            await unitOfWork.SaveChangesAsync();
            return Ok(tournamentId);
        }
    }
}