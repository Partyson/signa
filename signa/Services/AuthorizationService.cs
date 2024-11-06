using Mapster;
using signa.Dto.user;
using signa.Entities;
using signa.Helpers;
using signa.Interfaces;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;

namespace signa.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository userRepository;
    private readonly ILogger<AuthorizationService> logger;
    private readonly IJwtProvider jwtProvider;

    public AuthorizationService(IUserRepository userRepository, ILogger<AuthorizationService> logger, IJwtProvider jwtProvider)
    {
        this.userRepository = userRepository;
        this.logger = logger;
        this.jwtProvider = jwtProvider;
    }

    public async Task<string> RegisterUser(CreateUserDto newUser)
    {
        var newUserEntity = newUser.Adapt<UserEntity>();
        var addedUser = await userRepository.AddAsync(newUserEntity);
        logger.LogInformation($"User {addedUser.Id} created");
        return jwtProvider.GenerateToken(addedUser);
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