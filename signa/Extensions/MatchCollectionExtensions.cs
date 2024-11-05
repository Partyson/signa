using signa.Entities;

namespace signa.Extensions;

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