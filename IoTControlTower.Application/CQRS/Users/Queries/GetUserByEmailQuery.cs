using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Users.Queries;

public class GetUserByEmailQuery : IRequest<User>
{
    public string? Email { get; set; }
}
