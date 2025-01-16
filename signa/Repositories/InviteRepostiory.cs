using EntityFrameworkCore.Repository;
using signa.DataAccess;
using signa.Entities;
using signa.Interfaces.Repositories;

namespace signa.Repositories;

public class InviteRepository(ApplicationDbContext context) : Repository<InviteEntity>(context), IInviteRepository;
