using signa.Dto.team;
using signa.Entities;

namespace signa.Interfaces;

public interface ITeamRepository
{
    Task<Guid> Create(CreateTeamDto teamEntity);
    Task<TeamEntity?> Get(Guid teamId);
    Task<List<TeamEntity>> GetAll();
    Task<Guid> Update(TeamEntity newTeamEntity);
    Task<Guid> Delete(Guid teamId);
}