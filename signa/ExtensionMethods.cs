using EntityFrameworkCore.QueryBuilder.Interfaces;
using Microsoft.EntityFrameworkCore;
using signa.Entities;
using signa.Interfaces;

namespace signa;

public static class ExtensionMethods
{
    public static List<MatchEntity> AddTeams(this List<MatchEntity> matches, List<TeamEntity> teams)
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

    public static List<MatchEntity> ConnectMatches(this List<MatchEntity> matches)
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

        return matches;
    }

    internal static IMultipleResultQuery<MatchTeamEntity> SearchMatchById(this IMatchTeamRepository repository,
        Guid matchId)
    {
        return repository.MultipleResultQuery()
            .Include(x => 
                x.Include(x => x.Match)
                    .Include(x => x.Team))
            .AndFilter(x => x.Match.Id == matchId);
    }
}