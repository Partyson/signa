using AutoMapper;
using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
using signa.Dto;
using signa.Entities;
using signa.Models;

namespace signa.Repositories;

public class UserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var usersEntities = await _context.Users
            .AsNoTracking()
            .ToListAsync();
        //TODO: вынести маппер в отдельный класс чтобы не создавать для каждого репрозитория MapperConfiguration
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserEntity, User>();
        });
        
        var users = usersEntities
            .Select(b => config.CreateMapper().Map<UserEntity, User>(b)).ToList();
        return users;
    }

    public async Task<Guid> Create(CreateUserDto user)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateUserDto, UserEntity>();
        });
        var userEntity = config.CreateMapper().Map<CreateUserDto, UserEntity>(user);
        userEntity.Id = Guid.NewGuid();
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
        return userEntity.Id;
    }
}