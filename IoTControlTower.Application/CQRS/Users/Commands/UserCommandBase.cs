using MediatR;
using IoTControlTower.Domain.Enum;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Users.Commands;

public abstract class UserCommandBase : IRequest<User>
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }

    public string? Role { get; set; }
}
