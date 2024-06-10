using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Data.Repositories;
using IoTControlTower.Application.CQRS.Users.Commands;
using IoTControlTower.Application.CQRS.Users.Notifications;

namespace IoTControlTower.Application.CQRS.Users.CommandHandlers;

public class CreateUserCommandsHandler(UnitOfWork unitOfWork,
                                      IValidator<CreateUserCommand> validator,
                                      IMediator mediator,
                                      UserManager<User> userManager,
                                      RoleManager<IdentityRole> roleManager,
                                      ILogger<CreateUserCommandsHandler> logger) : IRequestHandler<CreateUserCommand, User>
{
    private readonly IMediator _mediator = mediator;
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ILogger<CreateUserCommandsHandler> _logger = logger;
    private readonly IValidator<CreateUserCommand> _validator = validator;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle() - Validating CreateUserCommand for email: {Email}", request.Email);
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

            var newUser = new User(
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
                request.Role,
                DateTime.Now
            );

            _logger.LogInformation("Handle() - Checking if user already exists for email: {Email}", request.Email);
            var user = await _unitOfWork.UserRepository.GetUserByEmail(request.Email);
            if (user is not null)
            {
                _logger.LogWarning("Handle() - User already exists with email: {Email}", request.Email);
                throw new Exception("User already exists.");
            }

            _logger.LogInformation("Handle() - Checking if email already exists: {Email}", request.Email);
            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists is not null)
            {
                _logger.LogWarning("Handle() - Email already exists: {Email}", request.Email);
                throw new Exception("Email already exists.");
            }

            _logger.LogInformation("Handle() - Checking if role exists: {Role}", request.Role);
            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                _logger.LogWarning("Handle() - Role does not exist: {Role}", request.Role);
                throw new Exception("Please choose a valid role for this user.");
            }

            _logger.LogInformation("Handle() - Creating user: {Email}", request.Email);
            var createUserResult = await _userManager.CreateAsync(newUser, request.Password);
            if (!createUserResult.Succeeded)
            {
                _logger.LogError("Handle() - Failed to create user: {Email}", request.Email);
                throw new Exception("Failed to create user.");
            }

            _logger.LogInformation("Handle() - Adding user to role: {Role}", request.Role);
            await _userManager.AddToRoleAsync(newUser, request.Role);

            _logger.LogInformation("Handle() - Publishing UserCreatedNotification for user: {Email}", request.Email);
            await _mediator.Publish(new UserCreatedNotification(newUser), cancellationToken);

            _logger.LogInformation("Handle() - User created successfully: {Email}", request.Email);
            return newUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Handle() - Error occurred while creating user: {Email}", request.Email);
            throw;
        }
    }
}
