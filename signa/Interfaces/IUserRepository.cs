using signa.Dto;
using signa.Entities;

namespace signa.Interfaces;

public interface IUserRepository
{
    Task<Guid> Create(UserEntity userEntity);
    Task<UserEntity?> Get(Guid userId);
    Task<Guid> Update(UserEntity newUserEntity);
    Task<Guid> Delete(Guid id);
}