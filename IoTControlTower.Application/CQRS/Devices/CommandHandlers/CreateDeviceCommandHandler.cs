using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Validation;
using IoTControlTower.Infra.Data.Repositories;
using IoTControlTower.Application.CQRS.Devices.Commands;
using IoTControlTower.Application.CQRS.Devices.Notifications;

namespace IoTControlTower.Application.CQRS.Devices.CommandHandlers;

public class CreateDeviceCommandHandler(UnitOfWork unitOfWork,
                                        IValidator<CreateDeviceCommand> validator,
                                        IMediator mediator,
                                        UserManager<User> userManager,
                                        ILogger<CreateDeviceCommandHandler> logger) : IRequestHandler<CreateDeviceCommand, Device>
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly UnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly ILogger<CreateDeviceCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IValidator<CreateDeviceCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Device> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateDeviceCommand for description: {Description}", request.Description);

        try
        {
            var user = await GetUserByEmailAsync(request.Email);
            request.UserId = user.Id;
            ValidateCommand(request);
            var newDevice = await CreateNewDeviceAsync(request, request.UserId);
            await PublishDeviceCreatedNotificationAsync(newDevice, cancellationToken);
            return newDevice;
        }
        catch (ValidationException ex)
        {
            HandleValidationException(ex);
            throw;
        }
        catch (Exception ex)
        {
            HandleGeneralException(ex);
            throw;
        }
    }

    private void ValidateCommand(CreateDeviceCommand request) => _validator.ValidateAndThrow(request);

    private async Task<Device> CreateNewDeviceAsync(CreateDeviceCommand request, string userId)
    {
        var newDevice = new Device(description: request.Description,
                                    manufacturer: request.Manufacturer,
                                    url: request.Url,
                                    isActive: request.IsActive,
                                    userId: userId,
                                    lastCommunication: request.LastCommunication,
                                    ipAddress: request.IpAddress,
                                    location: request.Location,
                                    firmwareVersion: request.FirmwareVersion,
                                    manufactureDate: request.ManufactureDate,
                                    serialNumber: request.SerialNumber,
                                    connectionType: request.ConnectionType,
                                    healthStatus: request.HealthStatus,
                                    lastKnownStatus: request.LastKnownStatus,
                                    owner: request.Owner,
                                    installationDate: request.InstallationDate,
                                    lastMaintenanceDate: request.LastMaintenanceDate,
                                    sensorType: request.SensorType,
                                    alarmSettings: request.AlarmSettings,
                                    lastHealthCheckDate: request.LastHealthCheckDate,
                                    maintenanceHistory: request.MaintenanceHistory);
        await AddDeviceToRepositoryAsync(newDevice);
        return newDevice;
    }

    private async Task PublishDeviceCreatedNotificationAsync(Device newDevice, CancellationToken cancellationToken) => await _mediator.Publish(new DeviceCreatedNotification(newDevice), cancellationToken);

    private async Task<User> GetUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email) ?? throw new DomainExceptions($"User not found for email: {email}");

    private async Task AddDeviceToRepositoryAsync(Device newDevice)
    {
        await _unitOfWork.DevicesRepository.AddDeviceAsync(newDevice);
        await _unitOfWork.CommitAsync();
    }

    private void HandleValidationException(ValidationException ex) => _logger.LogWarning(ex, "Validation failed for CreateDeviceCommand");

    private void HandleGeneralException(Exception ex) => _logger.LogError(ex, "Error occurred while handling CreateDeviceCommand");
}
