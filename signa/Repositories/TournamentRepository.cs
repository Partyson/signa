using EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
using signa.Entities;
using signa.Interfaces;

namespace signa.Repositories;

public class TournamentRepository(ApplicationDbContext context) : Repository<TournamentEntity>(context), ITournamentRepository;