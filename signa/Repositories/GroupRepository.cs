using EntityFrameworkCore.Repository;
using signa.DataAccess;
using signa.Entities;
using signa.Interfaces.Repositories;

namespace signa.Repositories;

public class GroupRepository(ApplicationDbContext context) : Repository<GroupEntity>(context), IGroupRepository;