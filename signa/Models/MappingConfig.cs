﻿using JetBrains.Annotations;
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
            .Map(dest => dest.TeamIds, src=> src.Teams.Count == 0 ? new List<Guid>() : src.Teams.Select(t => t.Id).ToList());
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
}