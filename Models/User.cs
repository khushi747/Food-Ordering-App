using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ordermanagement.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

//    public int Id { get; set; }

//    public required string Name { get; set; }

//    public required byte[] PasswordHash { get; set; }

//    public required byte[] PasswordSalt { get; set; }

//    public string Role { get; set; } = "user";

//    [EmailAddress(ErrorMessage = "Invalid email address.")]
//    public string? Email { get; set; }

//    public ICollection<Locker>? Lockers { get; set; }
}
