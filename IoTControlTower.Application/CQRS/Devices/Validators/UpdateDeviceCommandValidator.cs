using FluentValidation;
using IoTControlTower.Application.CQRS.Devices.Commands;

namespace IoTControlTower.Application.CQRS.Devices.Validators;

public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceCommandValidator()
    {
        RuleFor(device => device.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(device => device.Manufacturer).NotEmpty().WithMessage("Manufacturer is required.");
        RuleFor(device => device.Url).NotEmpty().WithMessage("URL is required.");
        RuleFor(device => device.UserId).NotEmpty().WithMessage("User is required.");
        RuleFor(device => device.ManufactureDate).NotNull().WithMessage("Manufacture date is required.");
        RuleFor(device => device.SerialNumber).NotEmpty().WithMessage("Serial number is required.");
        RuleFor(device => device.ConnectionType).NotEmpty().WithMessage("Connection type is required.");
        RuleFor(device => device.HealthStatus).NotEmpty().WithMessage("Health status is required.");
        RuleFor(device => device.LastKnownStatus).NotEmpty().WithMessage("Last known status is required.");
        RuleFor(device => device.Owner).NotEmpty().WithMessage("Owner is required.");
        RuleFor(device => device.InstallationDate).NotNull().WithMessage("Installation date is required.");
        RuleFor(device => device.LastMaintenanceDate).NotNull().WithMessage("Last maintenance date is required.");
        RuleFor(device => device.SensorType).NotEmpty().WithMessage("Sensor type is required.");
        RuleFor(device => device.AlarmSettings).NotEmpty().WithMessage("Alarm settings are required.");
        RuleFor(device => device.LastHealthCheckDate).NotNull().WithMessage("Last health check date is required.");
        RuleFor(device => device.MaintenanceHistory).NotEmpty().WithMessage("Maintenance history is required.");
    }
}
