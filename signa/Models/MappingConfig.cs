using JetBrains.Annotations;
using Mapster;
using signa.Dto;
using signa.Dto.team;
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
    }
}