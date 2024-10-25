using System.Text.RegularExpressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using signa.Dto.match;
using signa.Entities;
using signa.Interfaces;
using signa.Repositories;

namespace signa.Services;


public class MatchesService : IMatchesService
{
    private readonly IMatchRepository matchRepository;
    private readonly ILogger<MatchesService> logger;
    private readonly ITournamentsService tournamentsService;
    private readonly IMatchTeamsService matchTeamsService;

    public MatchesService(IMatchRepository matchRepository, ITournamentsService tournamentsService,
        IMatchTeamsService matchTeamsService, ILogger<MatchesService> logger)
    {
        this.matchRepository = matchRepository;
        this.tournamentsService = tournamentsService;
        this.matchTeamsService = matchTeamsService;
        this.logger = logger;
    }

    public async Task<List<MatchResponseDto>> GetMatchesByTournamentId(Guid tournamentId)
    {
        var query = matchRepository.MultipleResultQuery()
            .Include(x => x.Include(x => x.Teams))
            .AndFilter(x => x.Tournament.Id == tournamentId);
        var matches = await matchRepository.SearchAsync(query);
        if(matches.Count == 0)
            logger.LogWarning($"No matches found for tournament {tournamentId}");
        
        return matches.Select(m => m.Adapt<MatchResponseDto>()).ToList();
    }

    public async Task<List<Guid>> CreateMatchesForTournament(Guid tournamentId)
    {

        var tournament = await tournamentsService.GetTournament(tournamentId);
        var matches = Enumerable.Range(0, tournament.Teams.Count - 1)
            .Select(_ => new MatchEntity { Tournament = tournament }).ToList()
            .ConnectMatches()
            .AddTeams(tournament.Teams);

        await matchRepository.AddRangeAsync(matches);
        logger.LogInformation($"Created {matches.Count} matches for tournament {tournamentId}");
        return matches.Select(m => m.Id).ToList();
    }

    public async Task<Guid> UpdateResult(Guid matchId, UpdateMatchResultDto updateMatchResultDto)
    {
        var updatedMatchId = await matchTeamsService.UpdateResult(matchId, updateMatchResultDto.Teams);
        logger.LogInformation($"Updated match results {updatedMatchId}");
        return updatedMatchId;
    }
    
    public async Task<List<Guid>> SwapTeams(Guid tournamentId, MatchTeamDto matchTeam1, MatchTeamDto matchTeam2)
    {
        throw new NotImplementedException();
        // var swappedMatchesId = await matchRepository.SwapTeams(tournamentId, matchTeam1, matchTeam2);
        // return swappedMatchesId;

    }

    public async Task<Guid> FinishMatch(Guid matchId)
    {
        var nextMatchId = await matchTeamsService.FinishMatch(matchId);
        logger.LogInformation($"Finished match {matchId}. Winner team go to {nextMatchId}");
        return nextMatchId;
    }
}