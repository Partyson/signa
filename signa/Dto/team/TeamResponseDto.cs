using signa.Dto.user;

namespace signa.Dto.team;

public class TeamResponseDto
{
    public string Title { get; set; }
    
    public UserResponseDto Captain { get; set; }
    
    public List<UserResponseDto> Members { get; set; }
}