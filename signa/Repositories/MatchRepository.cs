using EntityFrameworkCore.Repository;
using signa.DataAccess;
using signa.Entities;
using signa.Interfaces.Repositories;

namespace signa.Repositories;



public class MatchRepository(ApplicationDbContext context) : Repository<MatchEntity>(context), IMatchRepository;