using JetBrains.Annotations;
using Mapster;
using signa.Dto.group;
using signa.Dto.invite;
using signa.Dto.match;
using signa.Dto.team;
using signa.Dto.tournament;
using signa.Dto.user;
using signa.Entities;

namespace signa.Helpers;

[UsedImplicitly]
public class MappingConfig
{
    public static void RegisterMappings()
    {
        var salt = PasswordHasher.GenerateSalt();
        TypeAdapterConfig<CreateUserDto, UserEntity>
            .NewConfig()
            .Map(dest => dest.PasswordHash, src => PasswordHasher.HashPassword(src.Password, salt))
            .Map(dest => dest.PasswordSalt, src => Convert.ToBase64String(salt));
        TypeAdapterConfig<UpdateUserPassDto, UserEntity>
            .NewConfig()
            .Map(dest => dest.PasswordHash, src => PasswordHasher.HashPassword(src.Password, salt))
            .Map(dest => dest.PasswordSalt, src => Convert.ToBase64String(salt));
        TypeAdapterConfig<UserEntity, UserSearchItemDto>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Patronymic, src => src.Patronymic)
            .Map(dest => dest.GroupNumber, src => src.GroupNumber)
            .IgnoreNonMapped(true);
            
        TypeAdapterConfig<TeamEntity, TeamResponseDto>
            .NewConfig()
            .Map(dest => dest.Captain, src => src.Captain.Adapt<UserResponseDto>())
            .Map(dest => dest.Members, src => src.Members.Select(m => m.Adapt<UserResponseDto>()));
        
        TypeAdapterConfig<MatchEntity, MatchResponseDto>
            .NewConfig()
            .Map(dest => dest.NextMatchId, src => src.NextMatch == null ? Guid.Empty : src.NextMatch.Id)
            .Map(dest => dest.Teams,
                src => src.Teams
                    .Select(team => CreateTeamInMatchDto(src, team))
                    .ToList()
            );
        
        TypeAdapterConfig<TournamentEntity, TournamentInfoDto>
            .NewConfig()
            .Map(dest => dest.Matches,
                src => src.Matches.Count == 0
                    ? new List<MatchResponseDto>()
                    : src.Matches.Select(m => m.Adapt<MatchResponseDto>()).ToList())
            .Map(dest => dest.Teams,
                src => src.Teams.Count == 0
                    ? new List<TeamResponseDto>()
                    : src.Teams.Select(t => t.Adapt<TeamResponseDto>()).ToList())
            .Map(dest => dest.CurrentMembersCount,
                src => src.Teams.Select(t => t.Members.Count).Sum())
            .Map(dest => dest.Members,
                src => src.Teams.SelectMany(t => t.Members).Adapt<List<UserResponseDto>>().ToList());

        TypeAdapterConfig<InviteEntity, InviteResponseDto>
            .NewConfig()
            .Map(dest => dest.WhoInviteFirstName, src => src.InviteTeam.Captain.FirstName)
            .Map(dest => dest.WhoInviteLastName, src => src.InviteTeam.Captain.LastName)
            .Map(dest => dest.TeamTitle, src => src.InviteTeam.Title);
        TypeAdapterConfig<InviteEntity, SentInviteDto>
            .NewConfig()
            .Map(dest => dest.State, src => src.State)
            .Map(dest => dest.InvitedUserFisrtName, src => src.InvitedUser.FirstName)
            .Map(dest => dest.InvitedUserLastName, src => src.InvitedUser.LastName);
        TypeAdapterConfig<GroupEntity, GroupResponseDto>
            .NewConfig()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Teams, src => CreateListTeamInGroupDto(src));
    }

    private static TeamInMatchResponseDto CreateTeamInMatchDto(MatchEntity match, TeamEntity team)
    {
        var teamInMatch = team
            .Adapt<TeamResponseDto>()
            .Adapt<TeamInMatchResponseDto>();
        teamInMatch.Score = team.MatchTeams
            .Where(mt => mt.Match.Id == match.Id)
            .Select(mt => mt.Score)
            .FirstOrDefault();
        return teamInMatch;
    }

    private static List<TeamInGroupResponseDto> CreateListTeamInGroupDto(GroupEntity group)
    {
        var teamsInGroup = new List<TeamInGroupResponseDto>();
        foreach (var team in group.Teams)
        {
            var groupMatchesIds = team.Matches
                .Where(m => m.Group != null)
                .Select(m => m.Id);
            var teamInGroup = team.Adapt<TeamResponseDto>().Adapt<TeamInGroupResponseDto>();
            teamInGroup.Score = team.MatchTeams
                .Where(mt => groupMatchesIds.Contains(mt.Match.Id))
                .Select(mt => mt.Score)
                .Sum();
            teamsInGroup.Add(teamInGroup);
        }
        return teamsInGroup;
    }
}