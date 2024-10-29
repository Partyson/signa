namespace signa.Dto.user;

public class UserSearchItemDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string GroupNumber { get; set; }
}