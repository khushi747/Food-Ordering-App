using System.ComponentModel.DataAnnotations;
 
namespace ordermanagement.Models;
 
 
public class Users
{
    public int Id{ get; set; }
 
    public required string Name { get; set; }
   
    public required byte[] PasswordHash { get; set; }
   
    public required byte[] PasswordSalt { get; set; }
   
    [RegularExpression("^(user|admin|chef)$", ErrorMessage = "Invalid TypeOfLocker")]
    public string Role { get; set; } = "user";
   
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }
   
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

 
}
 