namespace signa.Dto;

public class UserResponseDto
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string Gender { get; set; }
    
    public string GroupNumber { get; set; }
    
    public string Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? PhotoLink { get; set; }
}