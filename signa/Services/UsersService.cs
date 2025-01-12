using ErrorOr;
using JetBrains.Annotations;
using LinqKit;
using Mapster;
using signa.Dto.user;
using signa.Entities;
using signa.Interfaces;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;

namespace signa.Services;

[UsedImplicitly]
public class UsersService : IUsersService
{
    private readonly IUserRepository userRepository;
    private readonly ILogger<UsersService> logger;
    private readonly IJwtProvider jwtProvider;

    public UsersService(IUserRepository userRepository, ILogger<UsersService> logger, IJwtProvider jwtProvider)
    {
        this.userRepository = userRepository;
        this.logger = logger;
        this.jwtProvider = jwtProvider;
    }

    public async Task<UserResponseDto?> GetUserResponse(Guid userId)
    {
        var user = await GetUser(userId);
        return user.Value.Adapt<UserResponseDto>();
    }

    public async Task<ErrorOr<UserEntity>> GetUser(Guid userId)
    {
        var query = userRepository.SingleResultQuery()
            .AndFilter(x => !x.IsDeleted)
            .AndFilter(x => x.Id == userId);
        var userEntity = await userRepository.FirstOrDefaultAsync(query);
        if (userEntity == null)
        {
            logger.LogWarning("User not found from database");
            return Error.NotFound("General.NotFound", $"User {userId} not found");
        }

        logger.LogInformation($"User {userEntity.Id} is retrieved from database");
        return userEntity;
    }

    public async Task<List<UserEntity>> GetUserEntitiesByIds(List<Guid> userIds)
    {
        var query = userRepository.MultipleResultQuery()
            .AndFilter(x => userIds.Contains(x.Id));
        var userEntities = await userRepository.SearchAsync(query);
        if (userEntities.Count < userIds.Count)
            userIds
                .Except(userEntities.Select(x => x.Id))
                .ForEach(id => logger.LogWarning($"User {id} has not been found in database"));
        
        return userEntities.ToList();
    }

    public async Task<List<UserSearchItemDto>> GetUsersByPrefix(string prefix)
    {
        var query = userRepository.MultipleResultQuery()
            .AndFilter(x => x.FullName.Contains(prefix, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.FullName)
            .Page(1, 7);
        var foundedUsers = await userRepository.SearchAsync(query);
        return foundedUsers.Select(x => x.Adapt<UserSearchItemDto>()).ToList();
    }



    public async Task<Guid> UpdateUser(Guid userId, UpdateUserDto updateUser)
    {
        var query = userRepository.SingleResultQuery().AndFilter(x => x.Id == userId);
        var userEntity = await userRepository.FirstOrDefaultAsync(query);
        updateUser.Adapt(userEntity);
        userEntity.UpdatedAt = DateTime.Now;
        logger.LogInformation($"User {userId} updated");
        return userId;
    }

    public async Task<Guid> DeleteUser(Guid userId)
    {
        var query = userRepository.SingleResultQuery().AndFilter(x => x.Id == userId);
        var userEntity = await userRepository.FirstOrDefaultAsync(query);
        userEntity.IsDeleted = true;
        logger.LogInformation($"User {userId} deleted");
        return userId;
    }
}

