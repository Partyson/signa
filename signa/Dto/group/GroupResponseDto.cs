using signa.Dto.team;

namespace signa.Dto.group;

public class GroupResponseDto
{
    public List<TeamInGroupResponseDto> Teams { get; set; }
    public string Title { get; set; }
    
}