using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Users;
using IoTControlTower.Application.Users.Commands;
using IoTControlTower.Domain.Interface.UserRepository;

namespace IoTControlTower.Application.Service;

public class UserService(UserManager<User> userManager,
                         RoleManager<IdentityRole> roleManager,
                         IUserRepository userRepository,
                         IMapper mapper,
                         IMediator mediator,
                         ILogger<UserService> logger,
                         IEmailService emailService) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UserService> _logger = logger;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMediator _mediator = mediator;

    public async Task<bool> GetUserName(string userName)
    {
        _logger.LogInformation("GetUserName() - Retrieving user by username: {UserName}", userName);
        try
        {
            return await _userRepository.GetUserName(userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserName() - Error retrieving user by username: {UserName}", userName);
            throw;
        }
    }

    public async Task<string> GetUserId()
    {
        _logger.LogInformation("GetUserId() - Retrieving user ID");
        try
        {
            return await _userRepository.GetUserId();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserId() - Error retrieving user ID");
            throw;
        }
    }

    public async Task<UserDTO> GetUserData(UserDTO userDTO)
    {
        _logger.LogInformation("GetUserData() - Retrieving data for user: {UserName}", userDTO.UserName);
        try
        {
            var user = _mapper.Map<User>(userDTO);
            var dataUser = await _userRepository.GetUserData(user);
            return _mapper.Map<UserDTO>(dataUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserData() - Error retrieving data for user: {UserName}", userDTO.UserName);
            throw;
        }
    }

    public async Task<UserRegisterDTO> CreateUser(UserRegisterDTO userRegisterDTO)
    {
        try
        {
            _logger.LogInformation("CreateDevice");
            var createDeviceCommand = _mapper.Map<CreateUserCommand>(userRegisterDTO);
            await _mediator.Send(createDeviceCommand);
            return _mapper.Map<UserRegisterDTO>(createDeviceCommand);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<UserUpdateDTO> UpdateUser(UserUpdateDTO userUpdateDTO)
    {
        try
        {
            _logger.LogInformation("UpdateUser() - Updating user: {UserName}", userUpdateDTO.UserName);
            var user = _mapper.Map<User>(userUpdateDTO);
            var actualUser = await _userRepository.GetUserData(user) ?? throw new Exception("This user does not exist!");

            var originalUpdateDate = actualUser.UpdateDate;
            var originalLastLogin = actualUser.LastLogin;

            _mapper.Map(userUpdateDTO, actualUser);

            actualUser.UpdateDate ??= originalUpdateDate;
            actualUser.LastLogin ??= originalLastLogin;

            if (!string.IsNullOrEmpty(userUpdateDTO.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(actualUser);
                var resetResult = await _userManager.ResetPasswordAsync(actualUser, token, userUpdateDTO.Password);
                if (!resetResult.Succeeded)
                {
                    _logger.LogError("UpdateUser() - Failed to reset password for user: {UserName}", userUpdateDTO.UserName);
                    throw new Exception("Failed to reset password.");
                }
            }

            var updateResult = await _userManager.UpdateAsync(actualUser);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("UpdateUser() - Failed to update user: {UserName}", userUpdateDTO.UserName);
                throw new Exception("Failed to update user.");
            }

            var updatedUser = await _userRepository.GetUserData(actualUser);
            _logger.LogInformation("UpdateUser() - User updated successfully: {UserName}", userUpdateDTO.UserName);
            _logger.LogDebug("Updated user details: {@UpdatedUser}", updatedUser);

            return _mapper.Map<UserUpdateDTO>(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateUser() - Error updating user: {UserName}", userUpdateDTO.UserName);
            throw;
        }
    }
}
