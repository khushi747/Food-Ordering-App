using System;
using System.ComponentModel.DataAnnotations;


namespace Server.Dtos.UsersDto;

public class UserDto
{
    public int Id{ get; set; }

    public required string Name { get; set; }
    
    [RegularExpression("^(user|admin|cook)$", ErrorMessage = "Invalid TypeOfLocker")]
    public string Role { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

}
