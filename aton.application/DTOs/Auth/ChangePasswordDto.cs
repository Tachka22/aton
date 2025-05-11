using System.ComponentModel.DataAnnotations;

namespace aton.application.DTOs.Auth;

public class ChangePasswordDto
{
    [Required]
    public required string Login { get; set; }
    [Required]
    public required string NewPassword { get; set; }
}
