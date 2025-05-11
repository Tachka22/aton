using System.ComponentModel.DataAnnotations;

namespace aton.application.DTOs.Auth;

public class ChangeLoginDto
{
    [Required]
    public required string Login { get; set; } 
    [Required]
    public required string NewLogin { get; set; }
}
