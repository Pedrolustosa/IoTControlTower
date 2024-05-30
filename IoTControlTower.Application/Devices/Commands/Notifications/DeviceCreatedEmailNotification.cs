using MediatR;
using Microsoft.Extensions.Logging;

namespace IoTControlTower.Application.Devices.Commands.Notifications
{
    public class DeviceCreatedEmailNotification(ILogger<DeviceCreatedEmailNotification> logger) : INotificationHandler<DeviceCreatedNotification>
    {
        private readonly ILogger<DeviceCreatedEmailNotification> _logger = logger;

        public Task Handle(DeviceCreatedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Confirmation email ent for: { notification.Device.Id}");
            return Task.CompletedTask;
        }
    }
}
