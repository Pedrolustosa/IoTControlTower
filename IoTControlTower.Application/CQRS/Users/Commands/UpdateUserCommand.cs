using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Repository;

namespace IoTControlTower.Application.CQRS.Users.Commands;

public class UpdateUserCommand : UserCommandBase
{
    public Guid Id { get; set; }
}
