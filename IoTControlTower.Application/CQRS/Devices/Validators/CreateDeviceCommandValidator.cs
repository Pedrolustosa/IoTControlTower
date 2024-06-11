using FluentValidation;
using IoTControlTower.Application.CQRS.Devices.Commands;

namespace IoTControlTower.Application.CQRS.Devices.Validators
{
    public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
    {
        public CreateDeviceCommandValidator()
        {
            RuleFor(device => device.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");

            RuleFor(device => device.Manufacturer)
                .NotEmpty().WithMessage("Manufacturer is required.")
                .MaximumLength(50).WithMessage("Manufacturer must not exceed 50 characters.");

            RuleFor(device => device.Url)
                .NotEmpty().WithMessage("URL is required.")
                .MaximumLength(255).WithMessage("URL must not exceed 255 characters.")
                .Matches(@"^(http|https)://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$")
                .WithMessage("Invalid URL format.");

            RuleFor(device => device.UserId)
                .NotEmpty().WithMessage("User is required.");

            RuleFor(device => device.FirmwareVersion)
                .NotEmpty().WithMessage("Firmware version is required.")
                .MaximumLength(20).WithMessage("Firmware version must not exceed 20 characters.");

            RuleFor(device => device.IpAddress)
                .NotEmpty().WithMessage("IP address is required.")
                .Matches(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$")
                .WithMessage("Invalid IP address format.");

            RuleFor(device => device.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(50).WithMessage("Location must not exceed 50 characters.");

            RuleFor(device => device.AlarmSettings)
                .NotNull().WithMessage("Alarm settings are required.");

            RuleFor(device => device.LastCommunication)
                .NotNull().WithMessage("Last communication date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Last communication date cannot be in the future.");

            RuleFor(device => device.LastKnownStatus)
                .NotNull().WithMessage("Last known status is required.");

            RuleFor(device => device.LastMaintenanceDate)
                .NotNull().WithMessage("Last maintenance date is required.");

            RuleFor(device => device.MaintenanceHistory)
                .NotNull().WithMessage("Maintenance history is required.");

            RuleFor(device => device.InstallationDate)
                .NotNull().WithMessage("Installation date is required.");

            RuleFor(device => device.ManufactureDate)
                .NotNull().WithMessage("Manufacture date is required.");

            RuleFor(device => device.Owner)
                .NotEmpty().WithMessage("Owner is required.")
                .MaximumLength(50).WithMessage("Owner must not exceed 50 characters.");

            RuleFor(device => device.SerialNumber)
                .NotEmpty().WithMessage("Serial number is required.")
                .MaximumLength(20).WithMessage("Serial number must not exceed 20 characters.");
        }
    }
}