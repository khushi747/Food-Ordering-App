using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.UsersDto;

public class SignUpAdminDto
{
    public required string Name { get; set; }
    
    public required string Password { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [RegularExpression("^(admin)$", ErrorMessage = "Invalid TypeOfLocker")]
    public required string Role { get; set; }
}
