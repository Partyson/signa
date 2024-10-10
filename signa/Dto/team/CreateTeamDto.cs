namespace signa.Dto.team;

public class CreateTeamDto
{
    public string Title { get; set; }
    
    public Guid TournamentId { get; set; }
    
    public Guid CaptainId { get; set; }
    
    public List<Guid> MembersId { get; set; }
}