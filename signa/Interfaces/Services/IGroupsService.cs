using signa.Dto.group;

namespace signa.Interfaces.Services;

public interface IGroupsService
{
    Task<List<Guid>> CreateGroups(CreateGroupDto createGroupDto);
    Task<List<GroupResponseDto>> GetGroupsByTournamentId(Guid tournamentId);
}