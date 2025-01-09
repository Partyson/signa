using signa.Enums;

namespace signa.Entities;

public class InviteEntity : BaseEntity
{
    public TeamEntity? InviteTeam { get; set; }
    public UserEntity? InvitedUser { get; set; }

    public InviteState State { get; set; } = InviteState.Waiting;
}