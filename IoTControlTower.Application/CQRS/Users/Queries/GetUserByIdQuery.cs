using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Users.Queries;

public class GetUserByIdQuery : IRequest<User>
{
    public Guid Id { get; set; }
}
