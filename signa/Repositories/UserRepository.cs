using AutoMapper;
using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
using signa.Dto;
using signa.Entities;
using signa.Models;

namespace signa.Repositories;

public class UserRepository
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public UserRepository(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Guid> Create(UserEntity userEntity)
    {
        userEntity.Id = Guid.NewGuid();
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
        await context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.LastName, updateUserDto.LastName)
                .SetProperty(b => b.PhoneNumber, updateUserDto.PhoneNumber)
                .SetProperty(b => b.Email, updateUserDto.Email)
                .SetProperty(b => b.Password, updateUserDto.Password)
                .SetProperty(b => b.GroupNumber, updateUserDto.GroupNumber)
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