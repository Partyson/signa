using signa.Dto.user;

namespace signa.Interfaces.Services;

public interface IAuthorizationService
{
    Task<string?> RegisterUser(CreateUserDto newUser);
    Task<string> LoginUser(string email, string password);


}