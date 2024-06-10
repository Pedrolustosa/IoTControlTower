using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
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
            _validator.ValidateAndThrow(request);
            var user = await _userManager.FindByEmailAsync(request.Email);
            var newDevice = new Device(request.Description, request.Manufacturer, request.Url, request.IsActive, user.Id, request.LastCommunication, request.IpAddress, request.Location, request.FirmwareVersion);

            _logger.LogInformation("Adding new device to repository");
            await _unitOfWork.DevicesRepository.AddDeviceAsync(newDevice);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Publishing DeviceCreatedNotification for device: {DeviceId}", newDevice.Id);
            await _mediator.Publish(new DeviceCreatedNotification(newDevice), cancellationToken);

            _logger.LogInformation("CreateDeviceCommand handled successfully for device: {DeviceId}", newDevice.Id);
            return newDevice;
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed for CreateDeviceCommand");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling CreateDeviceCommand");
            throw;
        }
    }
}
