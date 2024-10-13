namespace signa.Dto.match;

public class MatchResponseDto
{
    public Guid Id { get; set; }
    
    public Guid NextMatchId { get; set; }
    
    public List<Guid> TeamIds { get; set; }
}