using EntityFrameworkCore.Repository;
using signa.DataAccess;
using signa.Entities;
using signa.Interfaces.Repositories;

namespace signa.Repositories;

public class TournamentRepository(ApplicationDbContext context) : Repository<TournamentEntity>(context), ITournamentRepository;