using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.UsersDto;

public class UpdateUserDto
{
    public string? Name { get; set; } 
    
    [RegularExpression("^(user|chef)$", ErrorMessage = "Invalid TypeOfUser")]
    public string? Role { get; set; } 
    
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }
    
    public bool? IsDeactivated { get; set; }
}
