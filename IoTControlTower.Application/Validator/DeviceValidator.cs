using FluentValidation;
using IoTControlTower.Application.DTO.Device;

namespace IoTControlTower.Application.Validator
{
    public class DeviceValidator : AbstractValidator<DeviceDTO>
    {
        public DeviceValidator()
        {
            RuleFor(device => device.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(device => device.Manufacturer).NotEmpty().WithMessage("Manufacturer is required.");
            RuleFor(device => device.Url).NotEmpty().WithMessage("URL is required.");
            RuleFor(device => device.UserId).NotEmpty().WithMessage("User ID is required.");
        }
    }
}
