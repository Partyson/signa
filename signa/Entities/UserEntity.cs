﻿namespace signa.Entities;

public class UserEntity : BaseEntity
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string GroupNumber { get; set; }
    
    public string Gender { get; set; }
    
    public string Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? PhotoLink { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string PasswordSalt { get; set; }
    

    public bool IsVerified { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public bool IsOrganized { get; set; }

    public List<TeamEntity> Teams { get; set; } = [];
    
    public List<TeamEntity> CaptainsTeams { get; set; } = [];
    
    public List<TournamentEntity> OrganizedTournaments { get; set; } = [];
}