using FluentValidation;
using IoTControlTower.Application.Users.Commands;

namespace IoTControlTower.Application.Validator;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(device => device.UserName).NotEmpty().WithMessage("UserName is required.");
        RuleFor(device => device.Password).NotEmpty().WithMessage("Password is required.");
        RuleFor(device => device.FullName).NotEmpty().WithMessage("FullName is required.");
    }
}
