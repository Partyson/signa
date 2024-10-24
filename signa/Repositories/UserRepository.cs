using EntityFrameworkCore.Repository;
using signa.DataAccess;
using signa.Entities;
using signa.Interfaces;

namespace signa.Repositories;

public class UserRepository(ApplicationDbContext context) : Repository<UserEntity>(context), IUserRepository;