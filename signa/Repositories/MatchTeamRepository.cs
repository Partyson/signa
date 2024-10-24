using EntityFrameworkCore.Repository;
using signa.DataAccess;
using signa.Entities;
using signa.Interfaces;

namespace signa.Repositories;

public class MatchTeamRepository(ApplicationDbContext context) : Repository<MatchTeamEntity>(context), IMatchTeamRepository;
