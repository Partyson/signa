using System.Text.RegularExpressions;
using Mapster;
using signa.Dto.match;
using signa.Entities;
using signa.Interfaces;
using signa.Repositories;

namespace signa.Services;


public class MatchesService : IMatchesService
{
    private readonly IMatchRepository matchRepository;
    private readonly ITournamentRepository tournamentRepository;

    public MatchesService(IMatchRepository matchRepository, ITournamentRepository tournamentRepository)
    {
        this.matchRepository = matchRepository;
        this.tournamentRepository = tournamentRepository;
    }

    public async Task<List<Guid>> CreateMatchesForTournament(Guid tournamentId)
    {
        var tournament = await tournamentRepository.Get(tournamentId);
        var matches = new List<MatchEntity>();
        var matchCount = tournament.Teams.Count - 1;
        for (var i = 0; i < matchCount; i++)
        {
            var matchId = Guid.NewGuid();
            var match = new MatchEntity
            {
                Id = matchId,
                Tournament = tournament,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            matches.Add(match);
        }

        ConnectMatches(matches);
        matches = AddTeams(matches, tournament.Teams);

        var matchesId = await matchRepository.CreateMatches(matches);
        return matchesId;
    }

    private static List<MatchEntity> AddTeams(List<MatchEntity> matches, List<TeamEntity> teams)
    {
        var matchIndex = 0;
        for (var i = 0; i < teams.Count; i++)
        {
            matches[matchIndex].Teams.Add(teams[i]);
            if (i % 2 != 0)
                matchIndex++;
        }

        return matches;
    }

    private static void ConnectMatches(List<MatchEntity> matches)
    {
        var roundCount = (int)Math.Log2(matches.Count + 1);
        var currentMatchIndex = 0;
        while (roundCount > 1)
        {
            var step = (int)Math.Pow(2, roundCount - 1);
            var matchInRound = step;
            for (var i = 0; i < matchInRound / 2; ++i)
            {
                matches[currentMatchIndex].NextMatch = matches[currentMatchIndex + step];
                matches[currentMatchIndex + 1].NextMatch = matches[currentMatchIndex + step];
                currentMatchIndex += 2;
                step--;
            }

            roundCount--;
        }
    }
    

}