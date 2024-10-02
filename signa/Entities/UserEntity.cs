namespace signa.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string GroupNumber { get; set; }
    
    public string Gender { get; set; }
    
    public string Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? PhotoLink { get; set; }
    
    public string Password { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    public bool IsVerified { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public bool IsOrganized { get; set; }
}