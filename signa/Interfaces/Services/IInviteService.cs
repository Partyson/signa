using signa.Dto.invite;
using signa.Enums;

namespace signa.Interfaces.Services;

public interface IInviteService
{
    Task<List<InviteResponseDto>> GetInvitesResponse(Guid invitedUserId);
    
    Task<List<SentInviteDto>> GetSentInvites(Guid captainId);
    Task<List<Guid>> CreateInvites(Guid teamId, List<Guid> invitedUsers);
    Task<Guid> AcceptInvite(Guid inviteId);
    Task<Guid> DiscardInvite(Guid inviteId);
}