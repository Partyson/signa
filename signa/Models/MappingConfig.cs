using JetBrains.Annotations;
using Mapster;
using signa.Dto.match;
using signa.Dto.team;
using signa.Dto.tournament;
using signa.Dto.user;
using signa.Entities;

namespace signa.Models;

[UsedImplicitly]
public class MappingConfig
{
    public static void RegisterMappings()
    {
        var salt = PasswordHasher.GenerateSalt();
        TypeAdapterConfig<CreateUserDto, UserEntity>
            .NewConfig()
            .Map(dest => dest.PasswordHash, src => PasswordHasher.HashPassword(src.Password, salt))
            .Map(dest => dest.PasswordSalt, src => System.Text.Encoding.Default.GetString(salt));
        TypeAdapterConfig<UpdateUserDto, UserEntity>
            .NewConfig()
            .Map(dest => dest.PasswordHash, src => PasswordHasher.HashPassword(src.Password, salt))
            .Map(dest => dest.PasswordSalt, src => System.Text.Encoding.Default.GetString(salt));
        TypeAdapterConfig<TeamEntity, TeamResponseDto>
            .NewConfig()
            .Map(dest => dest.Captain, src => src.Captain.Adapt<UserResponseDto>())
            .Map(dest => dest.Members, src => src.Members.Select(m => m.Adapt<UserResponseDto>()));
        TypeAdapterConfig<MatchEntity, MatchResponseDto>
            .NewConfig()
            .Map(dest => dest.NextMatchId, src => src.NextMatch == null ? Guid.Empty : src.NextMatch.Id)
            .Map(dest => dest.Teams,
                src => src.Teams.Count == 0 ? new List<TeamInMatchResponseDto>() :
                new List<TeamInMatchResponseDto>
                    {
                        CreateTeamInMatchDto(src, src.Teams[0]),
                        CreateTeamInMatchDto(src, src.Teams[1])
                    }
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
    }

    private static TeamInMatchResponseDto CreateTeamInMatchDto(MatchEntity match, TeamEntity team)
    {
        var teamInMatch = team
            .Adapt<TeamResponseDto>()
            .Adapt<TeamInMatchResponseDto>();
        teamInMatch.Id = team.Id;
        teamInMatch.Score = team.MatchTeams
            .Where(mt => mt.Match.Id == match.Id)
            .Select(mt => mt.Score)
            .FirstOrDefault();
        return teamInMatch;
    }
}