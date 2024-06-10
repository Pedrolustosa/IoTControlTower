using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Users;
using IoTControlTower.Application.CQRS.Users.Queries;
using IoTControlTower.Application.CQRS.Users.Commands;

namespace IoTControlTower.Application.Service;

public class UserService(IMapper mapper,
                         IMediator mediator,
                         ILogger<UserService> logger) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<UserDTO> GetUserByEmail(string email)
    {
        _logger.LogInformation("GetUserByEmail() - Attempting to get user by email: {Email}", email);
        try
        {
            var getUserByEmailQuery = new GetUserByEmailQuery { Email = email };
            if (getUserByEmailQuery is null)
            {
                _logger.LogError("GetUserByEmail() - Query object is null for email: {Email}", email);
                throw new ArgumentNullException(nameof(getUserByEmailQuery), "Query object cannot be null.");
            }

            var result = await _mediator.Send(getUserByEmailQuery);
            if (result is null)
            {
                _logger.LogWarning("GetUserByEmail() - User not found: {Email}", email);
                throw new Exception("User not found.");
            }

            var userDTO = _mapper.Map<UserDTO>(result);
            _logger.LogInformation("GetUserByEmail() - User found and mapped to DTO: {Email}", email);
            return userDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserByEmail() - Error occurred while getting user by email: {Email}", email);
            throw;
        }
    }

    public async Task<UserDTO> CreateUser(UserDTO userRegisterDTO)
    {
        _logger.LogInformation("CreateUser() - Attempting to create user: {Email}", userRegisterDTO.Email);
        try
        {
            var createUserCommand = _mapper.Map<CreateUserCommand>(userRegisterDTO);
            await _mediator.Send(createUserCommand);
            var createdUser = _mapper.Map<UserDTO>(createUserCommand);
            _logger.LogInformation("CreateUser() - User created successfully: {Email}", createdUser.Email);
            return createdUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUser() - Error occurred while creating user: {Email}", userRegisterDTO.Email);
            throw;
        }
    }

    public async Task<UserDTO> UpdateUser(UserDTO userUpdateDTO)
    {
        _logger.LogInformation("UpdateUser() - Attempting to update user: {Email}", userUpdateDTO.Email);
        try
        {
            var updateUserCommand = _mapper.Map<UpdateUserCommand>(userUpdateDTO);
            await _mediator.Send(updateUserCommand);
            var updatedUser = _mapper.Map<UserDTO>(updateUserCommand);
            _logger.LogInformation("UpdateUser() - User updated successfully: {Email}", updatedUser.Email);
            return updatedUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateUser() - Error occurred while updating user: {Email}", userUpdateDTO.Email);
            throw;
        }
    }
}
