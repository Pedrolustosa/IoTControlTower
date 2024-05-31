using IoTControlTower.Domain.Entities;
using MediatR;

namespace IoTControlTower.Application.CQRS.Users.Notifications;

public class UserCreatedNotification(User user) : INotification
{
    public User User { get; set; } = user;
}
