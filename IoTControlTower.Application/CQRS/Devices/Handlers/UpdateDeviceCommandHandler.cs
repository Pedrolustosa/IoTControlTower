﻿using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Repository;
using IoTControlTower.Application.CQRS.Devices.Commands;
using Microsoft.AspNetCore.Identity;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class UpdateDeviceCommandHandler(UnitOfWork unitOfWork,
                                        UserManager<User> userManager,
                                        ILogger<UpdateDeviceCommandHandler> logger) : IRequestHandler<UpdateDeviceCommand, Device>
{
    private readonly UnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly UserManager<User> _userManager = userManager  ?? throw new ArgumentNullException(nameof(userManager));
    private readonly ILogger<UpdateDeviceCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Device> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateDeviceCommand for device ID: {Id}", request.Id);

        try
        {
            var existingDevice = await _unitOfWork.DevicesRepository.GetDeviceByIdAsync(request.Id);

            if (existingDevice is null)
            {
                _logger.LogWarning("Device not found for ID: {Id}", request.Id);
                throw new InvalidOperationException("Device not found");
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            existingDevice.Update(request.Description, request.Manufacturer, request.Url, request.IsActive, user.Id);
            _unitOfWork.DevicesRepository.UpdateDeviceAsync(existingDevice);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("UpdateDeviceCommand handled successfully for device ID: {Id}", request.Id);
            return existingDevice;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operation failed for device ID: {Id}", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling UpdateDeviceCommand for device ID: {Id}", request.Id);
            throw;
        }
    }
}
