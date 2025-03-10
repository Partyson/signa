using ErrorOr;
using Mapster;
using signa.Dto.user;
using signa.Entities;
using signa.Helpers;
using signa.Interfaces;
using signa.Interfaces.Repositories;
using signa.Interfaces.Services;
using signa.Validators;

namespace signa.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository userRepository;
    private readonly ILogger<AuthorizationService> logger;
    private readonly IJwtProvider jwtProvider;

    public AuthorizationService(IUserRepository userRepository, ILogger<AuthorizationService> logger,
        IJwtProvider jwtProvider)
    {
        this.userRepository = userRepository;
        this.logger = logger;
        this.jwtProvider = jwtProvider;
    }

    public async Task<ErrorOr<string>> RegisterUser(CreateUserDto newUser)
    {
        var newUserEntity = newUser.Adapt<UserEntity>();
        
        var query = userRepository.SingleResultQuery()
            .AndFilter(x => x.Email == newUser.Email);
        var userWithCurrentEmail = await userRepository.FirstOrDefaultAsync(query);
        if (userWithCurrentEmail != null)
            return Error.Validation("General.Validation",
                $"Email {userWithCurrentEmail.Email} already exists");
        
        var addedUser = await userRepository.AddAsync(newUserEntity);
        logger.LogInformation($"User {addedUser.Id} created");
        return jwtProvider.GenerateToken(addedUser);
    }

    public async Task<ErrorOr<string>> LoginUser(string email, string password)
    {
        var query = userRepository.SingleResultQuery()
            .AndFilter(x => !x.IsDeleted)
            .AndFilter(x => x.Email == email);
        var userEntity = await userRepository.FirstOrDefaultAsync(query);
        if (userEntity == null)
            return Error.NotFound("General.NotFound", "Email not found");
        var passwordIsCorrect = PasswordHasher.VerifyPassword(password, userEntity.PasswordHash,
            Convert.FromBase64String(userEntity.PasswordSalt));
        if(!passwordIsCorrect)
            return Error.Validation("General.Validation", "Password is not correct");

        return jwtProvider.GenerateToken(userEntity);
    }
}