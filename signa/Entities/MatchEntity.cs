namespace signa.Entities;

public class MatchEntity : BaseEntity
{
    public TournamentEntity Tournament { get; set; }
    
    public MatchEntity? NextMatch { get; set; }

    public List<TeamEntity> Teams { get; set; } = [];
    
    public GroupEntity? Group { get; set; }

}