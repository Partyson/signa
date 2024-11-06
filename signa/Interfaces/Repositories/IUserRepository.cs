using EntityFrameworkCore.Repository.Interfaces;
using signa.Entities;

namespace signa.Interfaces.Repositories;

public interface IUserRepository : IRepository<UserEntity>;