using System.ComponentModel.DataAnnotations;

namespace aton.domain.Entities;

public class User
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Login должен содержать только латинские буквы и цифры.")]
    public string Login { get; set; }
    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Password должен содержать только латинские буквы и цифры.")]
    public string Password { get; set; }

    [Required]
    [RegularExpression("^[a-zA-Zа-яА-ЯёЁ]+$", ErrorMessage = "Name должен содержать только латинские и русские буквы.")]
    public string Name { get; set; }
    public int Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public bool Admin { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? RevokedOn { get; set; } = null;
    public string RevokedBy { get; set; } = string.Empty;

}
