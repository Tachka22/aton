using System.ComponentModel.DataAnnotations;

namespace aton.application.DTOs.Auth;

public class LoginDto
{
    [Required]
    public required string Login { get; set; }
    [Required]
    public required string Password { get; set; }
}
