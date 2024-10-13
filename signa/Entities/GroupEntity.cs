namespace signa.Entities;

public class GroupEntity : BaseEntity
{
    public List<TeamEntity> Teams { get; set; } = [];

    public TournamentEntity Tournament { get; set; } = null!;

    public List<MatchEntity> Matches { get; set; } = [];
    
    public string Title { get; set; }
}