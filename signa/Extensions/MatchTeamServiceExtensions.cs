using EntityFrameworkCore.QueryBuilder.Interfaces;
using Microsoft.EntityFrameworkCore;
using signa.Entities;
using signa.Interfaces;

namespace signa.Extensions;

public static class MatchTeamServiceExtensions
{
    internal static IMultipleResultQuery<MatchTeamEntity> SearchMatchByIdQuery(this IMatchTeamRepository repository,
        Guid matchId)
    {
        return repository.MultipleResultQuery()
            .Include(x => 
                x.Include(x => x.Match)
                    .Include(x => x.Team))
            .AndFilter(x => x.Match.Id == matchId);
    }
}