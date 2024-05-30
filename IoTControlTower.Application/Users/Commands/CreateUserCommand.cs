using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Repository;
using Microsoft.AspNetCore.Mvc.Routing;
using IoTControlTower.Application.DTO.Email;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.Application.Users.Commands;

public class CreateUserCommand : UserCommandBase
{
    public class CreateUserCommandsHandler(UnitOfWork unitOfWork, IValidator<CreateUserCommand> validator, IEmailService emailService, IUrlHelper iUrlHelper, UserManager<User> userManager,  RoleManager<IdentityRole> roleManager, ILogger<CreateUserCommandsHandler> logger) : IRequestHandler<CreateUserCommand, User>
    {
        private readonly UnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CreateUserCommandsHandler> _logger = logger;
        private readonly IValidator<CreateUserCommand> _validator = validator;
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IEmailService _emailService = emailService;
        private readonly IUrlHelper _iUrlHelper = iUrlHelper;

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var urlHelper = _iUrlHelper;
                var schema = _iUrlHelper.ActionContext.HttpContext.Request.Scheme;
                var newUser = new User
                    (
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
                    request.RegistrationDate
                    );
                var hasUser = await _unitOfWork.UserRepository.GetUserName(request.UserName);
                if (hasUser)
                {
                    _logger.LogWarning("CreateUser() - User already exists: {UserName}", request.UserName);
                    throw new Exception("User already exists.");
                }

                var userExist = await _userManager.FindByEmailAsync(request.Email);
                if (userExist != null)
                {
                    _logger.LogWarning("CreateUser() - Email already exists: {Email}", request.Email);
                    throw new Exception("Email already exists.");
                }

                if (await _roleManager.RoleExistsAsync(request.Role))
                {
                    var result = await _userManager.CreateAsync(newUser, request.Password);
                    if (!result.Succeeded)
                    {
                        _logger.LogError("CreateUser() - Failed to create user: {UserName}", request.UserName);
                        throw new Exception("Failed to create user.");
                    }

                    await _userManager.AddToRoleAsync(newUser, request.Role);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    string? confirmationLink = ConfirmationLink(newUser, urlHelper, schema.ToString(), token);

                    var message = new MessageDTO(new string[] { request.Email! }, "Confirmation email link", confirmationLink!);
                    _emailService.SendEmail(message);
                    _logger.LogInformation("CreateUser() - User created successfully: {UserName}", request.UserName);
                    return newUser;
                }
                else
                {
                    _logger.LogWarning("CreateUser() - Invalid role: {Role}", request.Role);
                    throw new Exception("Please choose a valid role for this user.");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ConfirmationLink(User newUser, IUrlHelper urlHelper, string scheme, string token)
        {
            _logger.LogInformation("ConfirmationLink() - Generating confirmation link for user with email: {Email}", newUser.Email);

            var confirmationLink = urlHelper.Action(new UrlActionContext
            {
                Action = nameof(ConfirmEmail),
                Controller = "Users",
                Values = new { token, email = newUser.Email },
                Protocol = scheme
            });

            if (confirmationLink is null)
            {
                _logger.LogError("ConfirmationLink() - Failed to generate confirmation link.");
                throw new Exception("Failed to generate confirmation link.");
            }

            _logger.LogInformation("ConfirmationLink() - Confirmation link generated successfully for user with email: {Email}", newUser.Email);

            return confirmationLink;
        }

        public async Task<string> ConfirmEmail(string token, string email)
        {
            _logger.LogInformation("ConfirmEmail() - Confirming email for user with email: {Email}", email);

            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("ConfirmEmail() - Email confirmed successfully for user with email: {Email}", email);
                        return "Success: Email verified successfully";
                    }
                }
                _logger.LogWarning("ConfirmEmail() - User with email {Email} does not exist", email);
                return "Error: This user does not exist.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ConfirmEmail() - An error occurred while confirming email for user with email: {Email}", email);
                throw;
            }
        }
    }
}
