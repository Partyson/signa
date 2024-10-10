using signa.Dto;
using signa.Dto.user;

namespace signa.Interfaces;

public interface IUsersService
{
    Task<UserResponseDto?> GetUser(Guid userId);
    Task<Guid> CreateUser(CreateUserDto newUser);
    Task<Guid> UpdateUser(Guid userId, UpdateUserDto updateUser);
    Task<Guid> DeleteUser(Guid userId);
}