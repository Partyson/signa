using ErrorOr;
using signa.Dto.user;

namespace signa.Interfaces.Services;

public interface IAuthorizationService
{
    Task<ErrorOr<string>> RegisterUser(CreateUserDto newUser);
    Task<ErrorOr<string>> LoginUser(string email, string password);


}