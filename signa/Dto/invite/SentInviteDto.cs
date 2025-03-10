using signa.Enums;

namespace signa.Dto.invite;

public class SentInviteDto
{
    public string InvitedUserFisrtName { get; set; }
    public string InvitedUserLastName { get; set; }
    public InviteState State { get; set; }
}