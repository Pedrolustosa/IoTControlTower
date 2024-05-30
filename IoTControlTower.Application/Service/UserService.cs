using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Users;
using IoTControlTower.Application.Users.Queries;
using IoTControlTower.Application.Users.Commands;
using IoTControlTower.Domain.Interface.UserRepository;

namespace IoTControlTower.Application.Service;

public class UserService(IUserRepository userRepository,
                         IMapper mapper,
                         IMediator mediator,
                         ILogger<UserService> logger) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UserService> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMediator _mediator = mediator;

    public async Task<bool> GetUserName(string userName)
    {
        try
        {
            _logger.LogInformation("GetUserName() - Checking if username exists: {UserName}", userName);
            var exists = await _userRepository.GetUserName(userName);
            _logger.LogInformation("GetUserName() - Username check result: {UserNameExists}", exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserName() - Error checking username: {UserName}", userName);
            throw;
        }
    }

    public async Task<AuthenticateDTO> GetUserData(AuthenticateDTO authenticateDTO)
    {
        try
        {
            _logger.LogInformation("GetUserData() - Retrieving data for user: {UserName}", authenticateDTO.UserName);
            var user = _mapper.Map<User>(authenticateDTO);
            var dataUser = await _userRepository.GetUserData(user);
            var userData = _mapper.Map<AuthenticateDTO>(dataUser);
            _logger.LogInformation("GetUserData() - User data retrieved successfully for user: {UserName}", authenticateDTO.UserName);
            return userData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserData() - Error retrieving data for user: {UserName}", authenticateDTO.UserName);
            throw;
        }
    }

    public async Task<UserRegisterDTO> CreateUser(UserRegisterDTO userRegisterDTO)
    {
        try
        {
            _logger.LogInformation("CreateUser() - Creating user: {UserName}", userRegisterDTO.UserName);
            var createUserCommand = _mapper.Map<CreateUserCommand>(userRegisterDTO);
            await _mediator.Send(createUserCommand);
            var createdUser = _mapper.Map<UserRegisterDTO>(createUserCommand);
            _logger.LogInformation("CreateUser() - User created successfully: {UserName}", userRegisterDTO.UserName);
            return createdUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUser() - Error creating user: {UserName}", userRegisterDTO.UserName);
            throw;
        }
    }

    public async Task<UserUpdateDTO> UpdateUser(UserUpdateDTO userUpdateDTO)
    {
        try
        {
            _logger.LogInformation("UpdateUser() - Updating user: {UserName}", userUpdateDTO.UserName);
            var updateUserCommand = _mapper.Map<UpdateUserCommand>(userUpdateDTO);
            var updatedUser = await _mediator.Send(updateUserCommand);
            var updatedUserDTO = _mapper.Map<UserUpdateDTO>(updatedUser);
            _logger.LogInformation("UpdateUser() - User updated successfully: {UserName}", userUpdateDTO.UserName);
            return updatedUserDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateUser() - Error updating user: {UserName}", userUpdateDTO.UserName);
            throw;
        }
    }

    public async Task<UserDTO> GetUserById(Guid userId)
    {
        try
        {
            _logger.LogInformation("GetUserById() - Retrieving user by ID: {UserId}", userId);
            var userByIdQuery = new GetUserByIdQuery { Id = userId };
            if (userByIdQuery is null)
            {
                _logger.LogWarning("GetUserById() - Query object is null for ID: {UserId}", userId);
                throw new ArgumentNullException(nameof(userByIdQuery), "Query object cannot be null.");
            }

            var result = await _mediator.Send(userByIdQuery);
            if (result is null)
            {
                _logger.LogWarning("GetUserById() - No user found with ID: {UserId}", userId);
                throw new Exception("User not found.");
            }

            var userDTO = _mapper.Map<UserDTO>(result);
            _logger.LogInformation("GetUserById() - User retrieved successfully by ID: {UserId}", userId);
            return userDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserById() - Error retrieving user by ID: {UserId}", userId);
            throw;
        }
    }
}
