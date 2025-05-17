namespace aton.application.DTOs.User;

public class GetUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime Birthday { get; set; } = DateTime.MinValue;
    public bool IsActive { get; set; } = false;
}
