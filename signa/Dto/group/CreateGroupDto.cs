namespace signa.Dto.group;

public class CreateGroupDto
{
    public Guid TournamentId { get; set; }
    public int GroupCount { get; set; }
    public List<Guid> TeamsIds { get; set; }
}