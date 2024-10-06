using JetBrains.Annotations;
using Mapster;
using signa.Dto;
using signa.Entities;
using signa.Interfaces;
using signa.Repositories;

namespace signa.Services;

[UsedImplicitly]
public class UsersService : IUsersService
{
    private readonly IUserRepository userRepository;

    public UsersService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<UserResponseDto?> GetUser(Guid userId)
    {
        var userEntity = await userRepository.Get(userId);
         return userEntity.Adapt<UserResponseDto>();
    }

    public async Task<Guid> CreateUser(CreateUserDto newUser)
    {
        var newUserEntity = newUser.Adapt<UserEntity>();
        newUserEntity.Id = Guid.NewGuid();
        newUserEntity.CreatedAt = DateTime.Now;
        newUserEntity.UpdatedAt = newUserEntity.CreatedAt;
        var id = await userRepository.Create(newUserEntity);
        return id;
    }

    public async Task<Guid> UpdateUser(Guid userId, UpdateUserDto updateUser)
    {
        var newUserEntity = updateUser.Adapt<UserEntity>();
        newUserEntity.Id = userId;
        var updateUserId = await userRepository.Update(newUserEntity);
        return updateUserId;
    }

    public async Task<Guid> DeleteUser(Guid userId)
    {
        var deletedUserId = await userRepository.Delete(userId);
        return deletedUserId;
    }
}