using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Repository;
using IoTControlTower.Application.CQRS.Devices.Commands;
using IoTControlTower.Application.CQRS.Devices.Notifications;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class CreateDeviceCommandHandler(UnitOfWork unitOfWork, 
                                        IValidator<CreateDeviceCommand> validator, 
                                        IMediator mediator, 
                                        ILogger<CreateDeviceCommandHandler> logger) : IRequestHandler<CreateDeviceCommand, Device>
{
    private readonly IMediator _mediator = mediator;
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreateDeviceCommandHandler> _logger = logger;
    private readonly IValidator<CreateDeviceCommand> _validator = validator;

    public async Task<Device> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("CreateDeviceCommand - Description: {Description}", request.Description);
            _validator.ValidateAndThrow(request);
            var newDevice = new Device(request.Description, request.Manufacturer, request.Url, request.IsActive);
            await _unitOfWork.DevicesRepository.AddDeviceAsync(newDevice);
            await _unitOfWork.CommitAsync();
            await _mediator.Publish(new DeviceCreatedNotification(newDevice), cancellationToken);
            return newDevice;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
