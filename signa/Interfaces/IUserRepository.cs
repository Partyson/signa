using EntityFrameworkCore.Repository.Interfaces;
using signa.Dto;
using signa.Entities;
using signa.Models;

namespace signa.Interfaces;

public interface IUserRepository : IRepository<UserEntity>;