using signa.Dto.group;
using ErrorOr;

namespace signa.Interfaces.Services;

public interface IGroupsService
{
    Task<ErrorOr<List<Guid>>> CreateGroups(CreateGroupDto createGroupDto);
    Task<ErrorOr<List<GroupResponseDto>>> GetGroupsByTournamentId(Guid tournamentId);
}