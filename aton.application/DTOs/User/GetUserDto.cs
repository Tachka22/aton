namespace aton.application.DTOs.User;

public class GetUserDto
{
    public string Name { get; set; }
    public string Gender { get; set; }
    public DateTime Birthday { get; set; }
    public bool IsActive { get; set; }
}
