using System;
using System.ComponentModel.DataAnnotations;


namespace server.Dtos.UsersDto;

public class SignUpDto
{
    public required string Name { get; set; }
    
    public required string Password { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [RegularExpression("^(user|chef)$", ErrorMessage = "Invalid TypeOfLocker")]
    public string Role { get; set; } = "user";
    
}
