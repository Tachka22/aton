namespace aton.application.DTOs.Auth;

public class RegisterDto
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required int Gender { get; set; }
    public DateTime Birthday { get; set; }

    public bool Admin { get; set; } = false;
}
