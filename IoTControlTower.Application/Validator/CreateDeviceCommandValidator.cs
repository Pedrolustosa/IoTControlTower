using FluentValidation;
using IoTControlTower.Application.CQRS.Devices.Commands;

namespace IoTControlTower.Application.Validator;

public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceCommandValidator()
    {
        RuleFor(device => device.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(device => device.Manufacturer).NotEmpty().WithMessage("Manufacturer is required.");
        RuleFor(device => device.Url).NotEmpty().WithMessage("URL is required.");
    }
}
