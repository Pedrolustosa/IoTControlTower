using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Validation;
using IoTControlTower.Infra.Data.Repositories;
using IoTControlTower.Application.CQRS.Devices.Commands;
using IoTControlTower.Domain.Interface.CachingRepository;

namespace IoTControlTower.Application.CQRS.Devices.CommandHandlers;

public class UpdateDeviceCommandHandler(UnitOfWork unitOfWork,
                                        ILogger<UpdateDeviceCommandHandler> logger,
                                        IValidator<UpdateDeviceCommand> validator,
                                        UserManager<User> userManager,
                                        ICachingRepository cachingRepository) : IRequestHandler<UpdateDeviceCommand, Device>
{
    private readonly UnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly ILogger<UpdateDeviceCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IValidator<UpdateDeviceCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    private readonly ICachingRepository _cachingRepository = cachingRepository ?? throw new ArgumentNullException(nameof(cachingRepository));

    public async Task<Device> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateDeviceCommand for device ID: {Id}", request.Id);

        try
        {
            _logger.LogInformation("Fetching user for email: {Email}", request.Email);
            var user = await GetUserByEmailAsync(request.Email);
            request.UserId = user.Id;

            _logger.LogInformation("Validating request for device ID: {Id}", request.Id);
            _validator.ValidateAndThrow(request);

            _logger.LogInformation("Fetching device for ID: {Id}", request.Id);
            var existingDevice = await _unitOfWork.DevicesRepository.GetDeviceByIdAsync(request.Id);
            if (existingDevice is null)
            {
                _logger.LogWarning("Device not found for ID: {Id}", request.Id);
                throw new InvalidOperationException("Device not found");
            }

            _logger.LogInformation("Updating device properties for ID: {Id}", request.Id);
            existingDevice.Update(request.Description, request.Manufacturer, request.Url, request.IsActive, request.UserId,
                request.LastCommunication, request.IpAddress, request.Location, request.FirmwareVersion, request.ManufactureDate,
                request.SerialNumber, request.ConnectionType, request.HealthStatus, request.LastKnownStatus, request.Owner,
                request.InstallationDate, request.LastMaintenanceDate, request.SensorType, request.AlarmSettings,
                request.LastHealthCheckDate, request.MaintenanceHistory);

            _logger.LogInformation("Committing changes for device ID: {Id}", request.Id);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Updating cache for device ID: {Id}", request.Id);
            await _cachingRepository.UpdateAsync(request.Id.ToString(), existingDevice.ToString());

            _logger.LogInformation("UpdateDeviceCommand handled successfully for device ID: {Id}", request.Id);
            return existingDevice;
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed for UpdateDeviceCommand for device ID: {Id}", request.Id);
            throw;
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

    private async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            _logger.LogWarning("User not found for email: {Email}", email);
            throw new DomainExceptions($"User not found for email: {email}");
        }
        return user;
    }
}
