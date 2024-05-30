using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Repository;

namespace IoTControlTower.Application.Users.Commands;

public class UpdateUserCommand : UserCommandBase
{
    public Guid Id { get; set; }

    public class UpdateUserCommandHandler(UnitOfWork unitOfWork, UserManager<User> userManager, ILogger<UpdateUserCommandHandler> logger) : IRequestHandler<UpdateUserCommand, User>
    {
        private readonly UnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ILogger<UpdateUserCommandHandler> _logger = logger;

        public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("UpdateUser() - Updating user: {UserName}", request.UserName);
                var existingUser = await _unitOfWork.UserRepository.GetUserById(request.Id)
                    ?? throw new InvalidOperationException("User not found");

                existingUser.Update(
                    request.FullName,
                    request.UserName,
                    request.Password,
                    request.Email,
                    request.DateOfBirth,
                    request.Gender,
                    request.PhoneNumber,
                    request.Address,
                    request.City,
                    request.State,
                    request.Country,
                    request.PostalCode,
                    DateTime.Now,
                    DateTime.Now,
                    existingUser.RegistrationDate,
                    DateTime.Now);

                if (!string.IsNullOrEmpty(request.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var resetResult = await _userManager.ResetPasswordAsync(existingUser, token, request.Password);
                    if (!resetResult.Succeeded)
                    {
                        _logger.LogError("UpdateUser() - Failed to reset password for user: {UserName}", existingUser.UserName);
                        throw new Exception("Failed to reset password.");
                    }
                }

                var updateResult = await _userManager.UpdateAsync(existingUser);
                if (!updateResult.Succeeded)
                {
                    _logger.LogError("UpdateUser() - Failed to update user: {UserName}", existingUser.UserName);
                    throw new Exception("Failed to update user.");
                }

                _logger.LogInformation("UpdateUser() - User updated successfully: {UserName}", existingUser.UserName);
                _logger.LogDebug("Updated user details: {@UpdatedUser}", existingUser);

                return existingUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateUser() - An error occurred while updating user: {UserName}", request.UserName);
                throw;
            }
        }
    }
}
