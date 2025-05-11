namespace aton.application.DTOs.User;

public class UpdateUserDto
{
    public required string Login { get; set; }
    public int Gender { get; set; } = 2;
    public string Name { get; set; } = string.Empty;
    public DateTime? Birthday { get; set; } 
}
