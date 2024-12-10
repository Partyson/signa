using signa.Enums;

namespace signa.Dto.tournament;

public class CreateTournamentDto
{
    public string Title { get; set; }
    
    public string Location { get; set; }
    
    public string SportType { get; set; }
    
    public int TeamsMembersMaxNumber { get; set; }
    
    public int TeamsMembersMinNumber { get; set; }
    
    public TournamentGender Gender { get; set; }
    
    public int MinFemaleCount { get; set; }
    
    public int MinMaleCount { get; set; }
    
    public int MaxTeamsCount { get; set; }
    
    public DateTime StartedAt { get; set; }
    
    public DateTime EndRegistrationAt { get; set; }
    
    public State State { get; set; }
    
    public string? RegulationLink { get; set; }

    public bool WithGroupStage { get; set; }
    
    public string? ChatLink { get; set; }
}