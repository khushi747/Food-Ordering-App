namespace Server.Dtos.UsersDto;
public class UserWithTokenDto
{
    public required string Name { get; set; }

    public required int Id { get; set; }

    public required string Token { get; set; }
}
