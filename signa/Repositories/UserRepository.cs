using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
using signa.Dto;
using signa.Entities;
using signa.Interfaces;
using signa.Models;

namespace signa.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext context;

    public UserRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Guid> Create(UserEntity userEntity)
    {
        await context.Users.AddAsync(userEntity);
        await context.SaveChangesAsync();
        return userEntity.Id;
    }

    public Task<UserEntity?> Get(Guid userId)
    {
        return Task.FromResult(context.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Id == userId));
    }

    public async Task<Guid> Update(UserEntity newUserEntity)
    {
        await context.Users
            .Where(u => u.Id == newUserEntity.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.PhoneNumber, newUserEntity.PhoneNumber)
                .SetProperty(b => b.Email, newUserEntity.Email)
                .SetProperty(b => b.PasswordSalt, newUserEntity.PasswordSalt)
                .SetProperty(b => b.PasswordHash, newUserEntity.PasswordHash)
                .SetProperty(b => b.PhotoLink, newUserEntity.PhotoLink));
        await context.SaveChangesAsync();
        return newUserEntity.Id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.IsDeleted, true));
        return id;
    }
}