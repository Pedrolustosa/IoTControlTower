using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using IoTControlTower.Application.DTO.Users;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService, 
                             ILogger<UsersController> logger, 
                             UserManager<User> userManager) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UsersController> _logger = logger;
    private readonly UserManager<User> _userManager = userManager;


    [HttpGet("GetUser/{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        _logger.LogInformation("Executing GetDeviceById({Id})", id);

        try
        {
            var device = await _userService.GetUserById(id);
            _logger.LogInformation("Successfully retrieved device with ID {Id}.", id);
            return device != null ? Ok(device) : NotFound("Device not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving device with ID {Id}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost("RegisterUser")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] UserRegisterDTO userRegisterDTO)
    {
        _logger.LogInformation("CreateUser() - Attempting to register a new user with email: {Email}", userRegisterDTO.Email);

        try
        {
            var createdUser = await _userService.CreateUser(userRegisterDTO);
            _logger.LogInformation("CreateUser() - User created successfully with email: {Email}", userRegisterDTO.Email);
            return Ok(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUser() - An error occurred while creating a user with email: {Email}", userRegisterDTO.Email);
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {ex.Message}");
        }
    }

    [HttpPut("UpdateUser")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO)
    {
        _logger.LogInformation("UpdateUser() - Attempting to update user: {UserName}", userUpdateDTO.UserName);

        try
        {
            var hasUser = await _userService.GetUserName(userUpdateDTO.UserName);

            if (!hasUser)
            {
                _logger.LogWarning("UpdateUser() - User does not exist: {UserName}", userUpdateDTO.UserName);
                return NotFound(new { message = "User does not exist." });
            }

            var applicationUserUpdate = await _userService.UpdateUser(userUpdateDTO);

            if (applicationUserUpdate is null)
            {
                _logger.LogWarning("UpdateUser() - No content returned for user: {UserName}", userUpdateDTO.UserName);
                return NoContent();
            }

            _logger.LogInformation("UpdateUser() - Successfully updated user: {UserName}", userUpdateDTO.UserName);
            return Ok(new { email = applicationUserUpdate.Email });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateUser() - Error occurred while updating user {UserName}.", userUpdateDTO.UserName);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }

    [HttpGet("ConfirmEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
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
                    return StatusCode(StatusCodes.Status200OK, new { Status = "Success", Message = "Email verified successfully" });
                }
                else
                {
                    _logger.LogWarning("ConfirmEmail() - Failed to confirm email for user with email: {Email}", email);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Failed to confirm email" });
                }
            }
            else
            {
                _logger.LogWarning("ConfirmEmail() - User with email {Email} does not exist", email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User does not exist" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ConfirmEmail() - An error occurred while confirming email for user with email: {Email}", email);
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "An error occurred while confirming email" });
        }
    }
}
