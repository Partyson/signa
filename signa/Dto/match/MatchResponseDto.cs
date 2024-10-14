namespace signa.Dto.match;

public class MatchResponseDto
{
    public Guid Id { get; set; }
    
    public Guid NextMatchId { get; set; }
    
    public List<string> TeamTitles { get; set; }
    
    public List<int> TeamScores { get; set; }
}