using signa.Dto;
using signa.Entities;

namespace signa.Interfaces;

public interface IUserRepository
{
    Task<Guid> Create(UserEntity userEntity);
    Task<UserEntity> GetById(Guid userId);
    Task<Guid> Update(Guid id, UpdateUserDto updateUserDto);
    Task<Guid> Delete(Guid id);
}