namespace signa.Entities;

public class MatchTeamEntity
{
    public MatchEntity? Match { get; set; }

    public TeamEntity? Team { get; set; }

    public int Score { get; set; }
}