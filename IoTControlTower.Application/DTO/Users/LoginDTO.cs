using System.ComponentModel.DataAnnotations;

namespace IoTControlTower.Application.DTO.Users;

public record LoginDTO
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid format email")]
    public required string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max " +
    "{1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public required string? Password { get; set; }
}
