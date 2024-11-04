using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using JetBrains.Annotations;
using LinqKit;
using Mapster;
using signa.Dto;
using signa.Dto.user;
using signa.Entities;
using signa.Interfaces;
using signa.Models;

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
        return user.Adapt<UserResponseDto>();
    }

    public async Task<UserEntity> GetUser(Guid userId)
    {
        var query = userRepository.SingleResultQuery()
            .AndFilter(x => !x.IsDeleted)
            .AndFilter(x => x.Id == userId);
        var userEntity = await userRepository.FirstOrDefaultAsync(query);
        if (userEntity == null)
        {
            logger.LogWarning("User not found from database");
            return null;
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

    public async Task<string> CreateUser(CreateUserDto newUser)
    {
        var newUserEntity = newUser.Adapt<UserEntity>();
        var addedUser = await userRepository.AddAsync(newUserEntity);
        logger.LogInformation($"User {addedUser.Id} created");
        return jwtProvider.GenerateToken(addedUser);
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

    public async Task<string> LoginUser(string email, string password)
    {
        var query = userRepository.SingleResultQuery()
            .AndFilter(x => !x.IsDeleted)
            .AndFilter(x => x.Email == email);
        var userEntity = await userRepository.FirstOrDefaultAsync(query);
        
        var passwordIsCorrect = PasswordHasher.VerifyPassword(password, userEntity.PasswordHash,
            Convert.FromBase64String(userEntity.PasswordSalt));
        if(!passwordIsCorrect)
            throw new UnauthorizedAccessException();

        return jwtProvider.GenerateToken(userEntity);
    }
}

