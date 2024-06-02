using IoTControlTower.Domain.Enum;

namespace IoTControlTower.Application.DTO.Users;

public record UserDTO
{
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string FullName { get; set; }
    public required DateTime? DateOfBirth { get; set; }
    public required Gender Gender { get; set; }
    public required string PhoneNumber { get; set; }

    public required string? Address { get; set; }
    public required string? City { get; set; }
    public required string? State { get; set; }
    public required string? Country { get; set; }
    public required string? PostalCode { get; set; }

    public required string? Role { get; set; }
}
