﻿namespace signa.Dto;

public class UpdateUserDto
{
    public string LastName { get; set; }
    
    public string GroupNumber { get; set; }
    
    public string Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string Password { get; set; }
    
    public string? PhotoLink { get; set; }
}