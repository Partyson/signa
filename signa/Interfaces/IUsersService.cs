﻿using signa.Dto;
using signa.Dto.user;
using signa.Entities;

namespace signa.Interfaces;

public interface IUsersService
{
    Task<UserResponseDto?> GetUserResponse(Guid userId);
    Task<UserEntity> GetUser(Guid userId);
    Task<List<UserEntity>> GetUserEntitiesByIds(List<Guid> userIds);
    Task<List<UserSearchItemDto>> GetUsersByPrefix(string prefix);
    Task<string> CreateUser(CreateUserDto newUser);
    Task<Guid> UpdateUser(Guid userId, UpdateUserDto updateUser);
    Task<Guid> DeleteUser(Guid userId);
    Task<string> LoginUser(string email, string password);

}