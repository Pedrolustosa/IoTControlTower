using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Data.Repositories;
using IoTControlTower.Application.CQRS.Users.Commands;

namespace IoTControlTower.Application.CQRS.Users.Handlers;

public class UpdateUserCommandHandler(UnitOfWork unitOfWork,
                                      UserManager<User> userManager,
                                      ILogger<UpdateUserCommandHandler> logger,
                                      IValidator<UpdateUserCommand> validator) : IRequestHandler<UpdateUserCommand, User>
{
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ILogger<UpdateUserCommandHandler> _logger = logger;
    private readonly IValidator<UpdateUserCommand> _validator = validator;

    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle() - Attempting to update user with email: {Email}", request.Email);
        try
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _unitOfWork.UserRepository.GetUserByEmail(request.Email)
                ?? throw new InvalidOperationException("User not found");

            _logger.LogInformation("Handle() - User found: {Email}", request.Email);

            var password = user.PasswordHash ??= request.Password;
            user.Update(
                request.FullName,
                request.UserName,
                password,
                request.Email,
                request.DateOfBirth,
                request.Gender,
                request.PhoneNumber,
                request.Address,
                request.City,
                request.State,
                request.Country,
                request.PostalCode,
                request.Role,
                DateTime.Now,
                DateTime.Now,
                user.RegistrationDate,
                DateTime.Now);

            if (!string.IsNullOrEmpty(request.Password))
            {
                _logger.LogInformation("Handle() - Resetting password for user: {Email}", request.Email);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (!resetResult.Succeeded)
                {
                    _logger.LogError("Handle() - Failed to reset password for user: {Email}", request.Email);
                    throw new Exception("Failed to reset password.");
                }
                _logger.LogInformation("Handle() - Password reset successfully for user: {Email}", request.Email);
            }

            _logger.LogInformation("Handle() - Updating user in database: {Email}", request.Email);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("Handle() - Failed to update user: {Email}", request.Email);
                throw new Exception("Failed to update user.");
            }
            _logger.LogInformation("Handle() - User updated successfully: {Email}", request.Email);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Handle() - Error occurred while updating user: {Email}", request.Email);
            throw;
        }
    }
}
