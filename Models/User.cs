using System;
using System.Collections.Generic;

namespace ordermanagement.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public string Role { get; set; } = null!;
}
