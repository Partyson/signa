
using Microsoft.EntityFrameworkCore;

namespace signa.Entities;

public class TeamEntity : BaseEntity
{
    public string Title { get; set; }

    public TournamentEntity Tournament { get; set; } = null!;
    
    //public GroupEntity Group { get; set; }

    public UserEntity Captain {get; set;} = null!;  
    
    public List<UserEntity> Members { get; set; } = [];
    
    
}