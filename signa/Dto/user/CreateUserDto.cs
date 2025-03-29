﻿using signa.Enums;

namespace signa.Dto.user;

public class CreateUserDto
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Patronymic { get; set; }
    
    public UserGender Gender { get; set; }
    
    public string GroupNumber { get; set; }
    
    public string Email { get; set; }
    
    public string? VkLink { get; set; }
    
    public string Password { get; set; }
    
}