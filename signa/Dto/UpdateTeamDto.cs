using signa.Entities;

namespace signa.Dto;

public class UpdateTeamDto
{
    public string Title { get; set; }
    
    public UserEntity Captain { get; set; }
    
    public List<UserEntity> Members { get; set; }
}