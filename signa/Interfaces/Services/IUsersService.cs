using signa.Dto.user;
using signa.Entities;

namespace signa.Interfaces.Services;

public interface IUsersService
{
    Task<UserResponseDto?> GetUserResponse(Guid userId);
    Task<UserEntity> GetUser(Guid userId);
    Task<List<UserEntity>> GetUserEntitiesByIds(List<Guid> userIds);
    Task<List<UserSearchItemDto>> GetUsersByPrefix(string prefix);
    Task<Guid> UpdateUser(Guid userId, UpdateUserDto updateUser);
    Task<Guid> DeleteUser(Guid userId);

}