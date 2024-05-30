using IoTControlTower.Domain.Entities;
using MediatR;

namespace IoTControlTower.Application.Users.Commands.Notifications;

public class UserCreatedNotification(User user) : INotification
{
    public User User { get; set; } = user;
}
