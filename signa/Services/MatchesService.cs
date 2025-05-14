﻿using Mapster;
using Microsoft.EntityFrameworkCore;
using signa.Dto.match;
using signa.Entities;
using signa.Extensions;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;
using ErrorOr;

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

    public async Task<ErrorOr<List<MatchResponseDto>>> GetMatchesByTournamentId(Guid tournamentId)
    {
        var query = matchRepository.MultipleResultQuery()
            .Include(x => x.Include(x => x.Teams))
            .AndFilter(x => x.Tournament.Id == tournamentId)
            .AndFilter(x => x.Group == null);
        var matches = await matchRepository.SearchAsync(query);
        if (matches.Count == 0)
        {
            logger.LogWarning($"No matches found for tournament {tournamentId}");
            return Error.NotFound("General.NotFound", $"No matches found for tournament {tournamentId}");
        }
        
        return matches.Select(m => m.Adapt<MatchResponseDto>()).ToList();
    }

    public async Task<ErrorOr<List<Guid>>> CreateMatchesForTournament(Guid tournamentId)
    {
        var tournament = await tournamentsService.GetTournament(tournamentId);

        if (tournament.IsError)
            return tournament.FirstError;
            
        var matches = Enumerable.Range(0, tournament.Value.Teams.Count - 1)
            .Select(_ => new MatchEntity { Tournament = tournament.Value })
            .ConnectMatches()
            .AddTeams(tournament.Value.Teams)
            .ToList();
        var groupMatches = CreateGroupMatches(tournament.Value.Groups, tournament.Value);
        matches = matches.Concat(groupMatches).ToList();
        await matchRepository.AddRangeAsync(matches);
        
        logger.LogInformation($"Created {matches.Count} matches for tournament {tournamentId}");
        return matches.Select(m => m.Id).ToList();
    }

    private static List<MatchEntity> CreateGroupMatches(List<GroupEntity> groups, TournamentEntity tournament)
    {
        var groupMatches = new List<MatchEntity>();
        foreach (var group in groups)
        {
            var groupTeams = group.Teams;
            var currentIndex = 0;
            while (currentIndex < groupTeams.Count - 1)
            {
                var step = 1;
                while (currentIndex + step < groupTeams.Count)
                {
                    groupMatches.Add(new MatchEntity
                    {
                        Tournament = tournament,
                        Group = group,
                        Teams = [groupTeams[currentIndex], groupTeams[currentIndex + step]]
                    });
                    step++;
                }
                currentIndex++;
            }
        }
        return groupMatches;
    }

    public async Task<ErrorOr<Guid>> UpdateResult(Guid matchId, UpdateMatchResultDto updateMatchResultDto)
    {
        var updatedMatchId = await matchTeamsService.UpdateResult(matchId, updateMatchResultDto.Teams);

        if (updatedMatchId.IsError)
            return updatedMatchId.FirstError;
        
        logger.LogInformation($"Updated match results {updatedMatchId}");
        return updatedMatchId.Value;
    }
    
    public async Task<ErrorOr<List<Guid>>> SwapTeams(MatchTeamDto matchTeam1, MatchTeamDto matchTeam2)
    {
        var query = matchRepository.MultipleResultQuery()
            .Include(x => x.Include(x => x.Teams))
            .AndFilter(x => x.Id == matchTeam1.MatchId || x.Id == matchTeam2.MatchId);
        var matches = await matchRepository.SearchAsync(query);

        if (matches.Count == 0)
            return Error.NotFound("General.NotFound", $"No matches found by ids {matchTeam1.MatchId} or {matchTeam2.MatchId}");
        
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

    public async Task<ErrorOr<Guid>> FinishMatch(Guid matchId)
    {
        var nextMatchId = await matchTeamsService.FinishMatch(matchId);

        if (nextMatchId.IsError)
            return nextMatchId.FirstError;
        
        logger.LogInformation($"Finished match {matchId}. Winner team go to {nextMatchId}");
        return nextMatchId.Value;
    }
}

