using signa.Dto.match;
using signa.Dto.team;
using signa.Dto.user;

namespace signa.Dto.tournament;

public class TournamentListItemDto
{
    public string Title { get; set; }
    
    public string Location { get; set; }
    
    public string SportType { get; set; }
    
    public string Gender { get; set; }
    
    public DateTime StartedAt { get; set; }
    
    public DateTime EndRegistrationAt { get; set; }
    
    public string State { get; set; }
}