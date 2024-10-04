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

    public async Task<Guid> Create(UserEntity userEntity, string password)
    {
        userEntity.Id = Guid.NewGuid();
        var newSalt = PasswordHasher.GenerateSalt();
        userEntity.PasswordHash = PasswordHasher.HashPassword(password, newSalt);
        userEntity.PasswordSalt = System.Text.Encoding.Default.GetString(newSalt);
        userEntity.CreatedAt = DateTime.Now;
        userEntity.UpdatedAt = userEntity.CreatedAt;
        await context.Users.AddAsync(userEntity);
        await context.SaveChangesAsync();
        return userEntity.Id;
    }

    public async Task<UserEntity> GetById(Guid userId)
    {
        return context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .FirstOrDefault();
    }

    public async Task<Guid> Update(Guid id, UpdateUserDto updateUserDto)
    {
        var newSalt = PasswordHasher.GenerateSalt();
        await context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.PhoneNumber, updateUserDto.PhoneNumber)
                .SetProperty(b => b.Email, updateUserDto.Email)
                .SetProperty(b => b.PasswordSalt, System.Text.Encoding.Default.GetString(newSalt))
                .SetProperty(b => b.PasswordHash, PasswordHasher.HashPassword(updateUserDto.Password, newSalt))
                .SetProperty(b => b.PhotoLink, updateUserDto.PhotoLink));
        await context.SaveChangesAsync();
        return id;
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