using IoTControlTower.Domain.Enum;

namespace IoTControlTower.Application.DTO.Users;

public record UserDTO(string? Email, string? UserName, string? FullName, DateTime? DateOfBirth, Gender? Gender,
                      string? PhoneNumber, string? Address, string? City, string? State, string? Country, string? PostalCode, string? Role)
{
    public string? Password { get; set; }
};
