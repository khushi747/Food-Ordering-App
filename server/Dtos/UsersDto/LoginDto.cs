using System;

namespace Server.Dtos.UsersDto;

public class LoginDto
{
    public required string Name { get; set; }
    public required string Password { get; set; }
}
