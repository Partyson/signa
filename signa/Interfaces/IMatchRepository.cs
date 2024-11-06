using EntityFrameworkCore.Repository.Interfaces;
using signa.Dto.match;
using signa.Entities;

namespace signa.Interfaces;

public interface IMatchRepository : IRepository<MatchEntity>;