namespace signa.Dto.team;

public class UpdateTeamDto : CreateTeamDto
{
    public List<Guid> MembersId { get; set; }
}