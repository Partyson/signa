using JetBrains.Annotations;
using Mapster;
using signa.Dto;
using signa.Entities;
using signa.Interfaces;

namespace signa.Services;

[UsedImplicitly]
public class UsersService : IUsersService
{
    private readonly IUserRepository userRepository;
    private readonly ILogger<UsersService> logger;

    public UsersService(IUserRepository userRepository, ILogger<UsersService> logger)
    {
        this.userRepository = userRepository;
        this.logger = logger;
    }

    public async Task<UserResponseDto?> GetUser(Guid userId)
    {
        var userEntity = await userRepository.Get(userId);
        if (userEntity != null)
        {
            logger.LogWarning($"User {userEntity.Id} is retrieved from database");
            return userEntity.Adapt<UserResponseDto>();
        }
        logger.LogInformation("User not found from database");
        return null;
    }

    public async Task<Guid> CreateUser(CreateUserDto newUser)
    {
        var newUserEntity = newUser.Adapt<UserEntity>();
        newUserEntity.Id = Guid.NewGuid();
        newUserEntity.CreatedAt = DateTime.Now;
        newUserEntity.UpdatedAt = newUserEntity.CreatedAt;
        var id = await userRepository.Create(newUserEntity);
        logger.LogInformation($"User {newUserEntity.Id} created");
        return id;
    }

    public async Task<Guid> UpdateUser(Guid userId, UpdateUserDto updateUser)
    {
        var newUserEntity = updateUser.Adapt<UserEntity>();
        newUserEntity.Id = userId;
        var updateUserId = await userRepository.Update(newUserEntity);
        logger.LogInformation($"User {userId} updated");
        return updateUserId;
    }

    public async Task<Guid> DeleteUser(Guid userId)
    {
        var deletedUserId = await userRepository.Delete(userId);
        logger.LogInformation($"User {userId} deleted");
        return deletedUserId;
    }
}