using FluentValidation;
using IoTControlTower.Domain.Enum;
using System.Text.RegularExpressions;
using IoTControlTower.Application.CQRS.Users.Commands;

namespace IoTControlTower.Application.Validator;

public partial class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("User name is required.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Must(ContainUpperCase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(ContainLowerCase).WithMessage("Password must contain at least one lowercase letter.")
            .Must(ContainDigit).WithMessage("Password must contain at least one digit.")
            .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character.");

        RuleFor(user => user.FullName)
            .NotEmpty().WithMessage("Full name is required.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(user => user.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{9}$").WithMessage("Phone number must be exactly 9 digits and numeric.");

        RuleFor(user => user.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeAtLeast18YearsOld).WithMessage("User must be at least 18 years old.");

        RuleFor(user => user.Gender)
            .NotEmpty().WithMessage("Gender is required.");

        RuleFor(user => user.Address)
            .NotEmpty().WithMessage("Address is required.");

        RuleFor(user => user.City)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(user => user.State)
            .NotEmpty().WithMessage("State is required.");

        RuleFor(user => user.Country)
            .NotEmpty().WithMessage("Country is required.");

        RuleFor(user => user.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.");

        RuleFor(user => user.Role)
            .NotEmpty().WithMessage("Role is required.");
    }
    private bool BeAtLeast18YearsOld(DateTime? dateOfBirth)
    {
        if (dateOfBirth is null) return false;
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }

    private bool ContainUpperCase(string password)
    {
        return password.Any(char.IsUpper);
    }

    private bool ContainLowerCase(string password)
    {
        return password.Any(char.IsLower);
    }

    private bool ContainDigit(string password)
    {
        return password.Any(char.IsDigit);
    }

    private bool ContainSpecialCharacter(string password)
    {
        return SpecialCharacter().IsMatch(password);
    }

    [GeneratedRegex(@"[\W_]")]
    private static partial Regex SpecialCharacter();
}
