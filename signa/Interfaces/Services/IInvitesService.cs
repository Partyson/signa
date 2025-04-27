using signa.Dto.invite;
using signa.Enums;
using ErrorOr;

namespace signa.Interfaces.Services;

public interface IInvitesService
{
    Task<ErrorOr<List<InviteResponseDto>>> GetInvitesResponse(Guid invitedUserId);
    
    Task<ErrorOr<List<SentInviteDto>>> GetSentInvites(Guid captainId);
    Task<ErrorOr<List<Guid>>> CreateInvites(Guid teamId, List<Guid> invitedUsers, Guid currentUserId);
    Task<ErrorOr<Guid>> AcceptInvite(Guid inviteId, Guid currentUserId);
    Task<ErrorOr<Guid>> DiscardInvite(Guid inviteId, Guid currentUserId);
}