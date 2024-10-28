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
            .Select(_ => new MatchEntity { Tournament = tournament })
            .ConnectMatches()
            .AddTeams(tournament.Teams)
            .ToList();

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
    
    public async Task<List<Guid>> SwapTeams(MatchTeamDto matchTeam1, MatchTeamDto matchTeam2)
    {
        var query = matchRepository.MultipleResultQuery()
            .Include(x => x.Include(x => x.Teams))
            .AndFilter(x => x.Id == matchTeam1.MatchId || x.Id == matchTeam2.MatchId);
        var matches = await matchRepository.SearchAsync(query);
        //TODO нужна консультация как сделать это красивше
        var match1Teams = matches.FirstOrDefault(m => m.Id == matchTeam1.MatchId).Teams;
        var match2Teams = matches.FirstOrDefault(m => m.Id == matchTeam2.MatchId).Teams;
        var match1TeamToRemove = match1Teams.FirstOrDefault(m => m.Id == matchTeam1.TeamId);
        var match2TeamToRemove = match2Teams.FirstOrDefault(m => m.Id == matchTeam2.TeamId);
        match1Teams.Remove(match1TeamToRemove);
        match1Teams.Add(match2TeamToRemove);
        match2Teams.Remove(match2TeamToRemove);
        match1Teams.Add(match1TeamToRemove);
        return matches.Select(m => m.Id).ToList();
    }

    public async Task<Guid> FinishMatch(Guid matchId)
    {
        var nextMatchId = await matchTeamsService.FinishMatch(matchId);
        logger.LogInformation($"Finished match {matchId}. Winner team go to {nextMatchId}");
        return nextMatchId;
    }
}

public static class MatchCollectionExtensions
{
    internal static IEnumerable<MatchEntity> AddTeams(this IEnumerable<MatchEntity> matches, List<TeamEntity> teams)
    {
        var matchIndex = 0;
        var matchEntities = matches.ToList();
        for (var i = 0; i < teams.Count; i++)
        {
            matchEntities[matchIndex].Teams.Add(teams[i]);
            if (i % 2 != 0)
                matchIndex++;
        }

        return matchEntities;
    }

    internal static IEnumerable<MatchEntity> ConnectMatches(this IEnumerable<MatchEntity> matches)
    {
        var matchEntities = matches.ToList();
        var roundCount = (int)Math.Log2(matchEntities.Count + 1);
        var currentMatchIndex = 0;
        while (roundCount > 1)
        {
            var step = (int)Math.Pow(2, roundCount - 1);
            var matchInRound = step;
            for (var i = 0; i < matchInRound / 2; ++i)
            {
                matchEntities[currentMatchIndex].NextMatch = matchEntities[currentMatchIndex + step];
                matchEntities[currentMatchIndex + 1].NextMatch = matchEntities[currentMatchIndex + step];
                currentMatchIndex += 2;
                step--;
            }

            roundCount--;
        }

        return matchEntities;
    }
}