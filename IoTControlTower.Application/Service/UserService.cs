using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface;
using IoTControlTower.Application.DTO.User;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.Application.Service
{
    public class UserService(UserManager<User> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IUserRepository userRepository,
                                        IMapper mapper,
                                        ILogger<UserService> logger) : IUserService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<bool> GetUserName(string userName)
        {
            _logger.LogInformation("GetUserName - Retrieving user by username: {UserName}", userName);
            try
            {
                return await _userRepository.GetUserName(userName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserName - Error retrieving user by username: {UserName}", userName);
                throw;
            }
        }

        public async Task<string> GetUserId()
        {
            _logger.LogInformation("GetUserId - Retrieving user ID");
            try
            {
                return await _userRepository.GetUserId();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserId - Error retrieving user ID");
                throw;
            }
        }

        public async Task<UserDTO> GetUserData(UserDTO userDTO)
        {
            _logger.LogInformation("GetUserData - Retrieving data for user: {UserName}", userDTO.UserName);
            try
            {
                var user = _mapper.Map<User>(userDTO);
                var dataUser = await _userRepository.GetUserData(user);
                return _mapper.Map<UserDTO>(dataUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserData - Error retrieving data for user: {UserName}", userDTO.UserName);
                throw;
            }
        }

        public async Task<bool> CreateUser(UserRegisterDTO userRegisterDTO, string role)
        {
            _logger.LogInformation("CreateUser - Creating user: {UserName}", userRegisterDTO.UserName);
            try
            {
                var hasUser = await GetUserName(userRegisterDTO.UserName);
                if (hasUser)
                {
                    _logger.LogWarning("CreateUser - User already exists: {UserName}", userRegisterDTO.UserName);
                    throw new Exception("User already exists.");
                }

                var user = _mapper.Map<User>(userRegisterDTO);
                var userExist = await _userManager.FindByEmailAsync(user.Email);
                if (userExist != null)
                {
                    _logger.LogWarning("CreateUser - Email already exists: {Email}", user.Email);
                    throw new Exception("Email already exists.");
                }

                if (await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _userManager.CreateAsync(user, userRegisterDTO.Password);
                    if (!result.Succeeded)
                    {
                        _logger.LogError("CreateUser - Failed to create user: {UserName}", userRegisterDTO.UserName);
                        throw new Exception("Failed to create user.");
                    }

                    await _userManager.AddToRoleAsync(user, role);
                    _logger.LogInformation("CreateUser - User created successfully: {UserName}", userRegisterDTO.UserName);
                    return true;
                }
                else
                {
                    _logger.LogWarning("CreateUser - Invalid role: {Role}", role);
                    throw new Exception("Please choose a valid role for this user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUser - Error creating user: {UserName}", userRegisterDTO.UserName);
                throw;
            }
        }

        public async Task<UserUpdateDTO> UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            _logger.LogInformation("UpdateUser - Updating user: {UserName}", userUpdateDTO.UserName);
            try
            {
                var user = _mapper.Map<User>(userUpdateDTO);
                var actualUser = await _userRepository.GetUserData(user) ?? throw new Exception("This user does not exist!");

                _mapper.Map(userUpdateDTO, actualUser);
                if (userUpdateDTO.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(actualUser);
                    var resetResult = await _userManager.ResetPasswordAsync(actualUser, token, userUpdateDTO.Password);
                    if (!resetResult.Succeeded)
                    {
                        _logger.LogError("UpdateUser - Failed to reset password for user: {UserName}", userUpdateDTO.UserName);
                        throw new Exception("Failed to reset password.");
                    }
                }

                var updateResult = await _userManager.UpdateAsync(actualUser);
                if (!updateResult.Succeeded)
                {
                    _logger.LogError("UpdateUser - Failed to update user: {UserName}", userUpdateDTO.UserName);
                    throw new Exception("Failed to update user.");
                }

                var updatedUser = await _userRepository.GetUserData(actualUser);
                _logger.LogInformation("UpdateUser - User updated successfully: {UserName}", userUpdateDTO.UserName);
                return _mapper.Map<UserUpdateDTO>(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateUser - Error updating user: {UserName}", userUpdateDTO.UserName);
                throw;
            }
        }
    }
}
