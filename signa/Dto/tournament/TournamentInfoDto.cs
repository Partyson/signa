using signa.Dto.match;
using signa.Dto.team;
using signa.Dto.user;

namespace signa.Dto.tournament;

public class TournamentInfoDto
{
    public string Title { get; set; }
    
    public string Location { get; set; }
    
    public string SportType { get; set; }
    
    public string Gender { get; set; }
    
    public int MinFemaleCount { get; set; }
    
    public int MinMaleCount { get; set; }
    
    public int MaxTeamsCount { get; set; }
    
    public int TeamsMembersMaxNumber { get; set; }
    
    public DateTime StartedAt { get; set; }
    
    public DateTime EndRegistrationAt { get; set; }
    
    public string State { get; set; }
    
    public string? RegulationLink { get; set; }
    
    public string? ChatLink { get; set; }
    
    public List<TeamResponseDto> Teams { get; set; }
    
    public List<UserResponseDto> Members { get; set; }
    
    public List<MatchResponseDto> Matches { get; set; }
    
    public List<UserResponseDto> Organizers { get; set; }
    
    public int CurrentMembersCount { get; set; }
}