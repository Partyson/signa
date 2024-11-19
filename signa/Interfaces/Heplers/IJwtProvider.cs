using signa.Entities;

namespace signa.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(UserEntity user);
}