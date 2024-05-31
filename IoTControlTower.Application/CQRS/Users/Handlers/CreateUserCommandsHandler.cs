using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Repository;
using IoTControlTower.Application.CQRS.Users.Commands;
using IoTControlTower.Application.CQRS.Users.Notifications;

namespace IoTControlTower.Application.CQRS.Users.Handlers;

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
        try
        {
            _validator.ValidateAndThrow(request);

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
                DateTime.Now
            );

            var userExists = await _unitOfWork.UserRepository.GetUserName(request.UserName);
            if (userExists)
            {
                _logger.LogWarning("CreateUser() - User already exists: {UserName}", request.UserName);
                throw new Exception("User already exists.");
            }

            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists != null)
            {
                _logger.LogWarning("CreateUser() - Email already exists: {Email}", request.Email);
                throw new Exception("Email already exists.");
            }

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                _logger.LogWarning("CreateUser() - Invalid role: {Role}", request.Role);
                throw new Exception("Please choose a valid role for this user.");
            }

            var createUserResult = await _userManager.CreateAsync(newUser, request.Password);
            if (!createUserResult.Succeeded)
            {
                _logger.LogError("CreateUser() - Failed to create user: {UserName}", request.UserName);
                throw new Exception("Failed to create user.");
            }

            await _mediator.Publish(new UserCreatedNotification(newUser), cancellationToken);
            _logger.LogInformation("CreateUser() - User created successfully: {UserName}", request.UserName);
            return newUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUser() - An error occurred while creating user: {UserName}", request.UserName);
            throw;
        }
    }
}
