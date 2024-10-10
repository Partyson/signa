namespace signa.Entities;

public class GroupEntity(Guid id, DateTime createdAt) : BaseEntity(id, createdAt)
{
    public List<TeamEntity> Teams { get; set; } = [];
    
    public TournamentEntity Tournament { get; set; }
    
    public string Title { get; set; }
}