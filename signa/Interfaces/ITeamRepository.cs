using EntityFrameworkCore.Repository;
using EntityFrameworkCore.Repository.Interfaces;
using signa.Dto.team;
using signa.Entities;

namespace signa.Interfaces;

public interface ITeamRepository : IRepository<TeamEntity>;