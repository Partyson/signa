using signa.Entities;

namespace signa.Dto;

public class CreateTeamDto
{
    public string Title { get; set; }
    
    public TournamentEntity Tournament { get; set; }
    
    public UserEntity Captain { get; set; }
    
    public List<UserEntity> Members { get; set; }
}