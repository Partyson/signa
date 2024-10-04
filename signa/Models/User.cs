using System.Security.Claims;
using System.Text;

namespace signa.Models;

public class User
{
    public const int MAX_FIRST_NAME_LENGTH = 15;
    public const int MAX_LAST_NAME_LENGTH = 15;
    public const int MAX_PATRONYMIC_LENGTH = 15;
    public const int VARCHAR_LIMIT = 255;
        
    public User (Guid id, string firstName, string lastName,
        string patronymic, string gender, string email, string groupNumber,
        string passwordHash, string passwordSalt, string? phoneNumber = null)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
        Gender = gender;
        Email = email;
        PhoneNumber = phoneNumber;
        GroupNumber = groupNumber;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        CreatedAt = DateTime.Now;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; set; }
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string Gender { get; set; }
    
    public string GroupNumber { get; set; }
    
    public string Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string PasswordSalt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public bool IsVerified { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public bool IsOrganized { get; set; }
    
    public string? PhotoLink { get; set; }
    
}