namespace signa.Dto.team;

public class TeamInMatchResponseDto : TeamResponseDto
{
    public Guid Id { get; set; }
    public int Score { get; set; }
}