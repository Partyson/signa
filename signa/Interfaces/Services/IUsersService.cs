using ErrorOr;
using signa.Dto.user;
using signa.Entities;

namespace signa.Interfaces.Services;

public interface IUsersService
{
    Task<ErrorOr<UserResponseDto?>> GetUserResponse(Guid userId);
    Task<ErrorOr<UserEntity>> GetUser(Guid userId);
    Task<ErrorOr<List<UserEntity>>> GetUserEntitiesByIds(List<Guid> userIds);
    Task<ErrorOr<List<UserSearchItemDto>>> GetUsersByPrefix(string prefix);
    Task<ErrorOr<Guid>> UpdateUser(Guid userId, UpdateUserDto updateUser);
    Task<ErrorOr<Guid>> DeleteUser(Guid userId);

}