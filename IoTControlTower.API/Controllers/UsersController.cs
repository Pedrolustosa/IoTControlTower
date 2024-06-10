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
                             UserManager<User> userManager) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UsersController> _logger = logger;
    private readonly UserManager<User> _userManager = userManager;

    [HttpGet("GetUserByEmail/{email}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        _logger.LogInformation("GetUserByEmail() - Attempting to get user by email: {Email}", email);
        try
        {
            var user = await _userService.GetUserByEmail(email);
            if (user is not null)
            {
                _logger.LogInformation("GetUserByEmail() - User found: {Email}", email);
                return Ok(user);
            }
            else
            {
                _logger.LogWarning("GetUserByEmail() - User not found: {Email}", email);
                return NotFound("User not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserByEmail() - Error occurred while getting user by email: {Email}", email);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost("CreateUser")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO userRegisterDTO)
    {
        _logger.LogInformation("CreateUser() - Attempting to create user: {Email}", userRegisterDTO.Email);
        try
        {
            var createdUser = await _userService.CreateUser(userRegisterDTO);
            _logger.LogInformation("CreateUser() - User created successfully: {Email}", createdUser.Email);
            return Ok(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUser() - Error occurred while creating user: {Email}", userRegisterDTO.Email);
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }

    [HttpPut("UpdateUser")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> UpdateUser([FromBody] UserDTO userUpdateDTO)
    {
        _logger.LogInformation("UpdateUser() - Attempting to update user: {Email}", userUpdateDTO.Email);
        try
        {
            var user = await _userService.GetUserByEmail(userUpdateDTO.Email);
            if (user is null)
            {
                _logger.LogWarning("UpdateUser() - User not found: {Email}", userUpdateDTO.Email);
                return NotFound(new { message = "User does not exist." });
            }

            var updatedUser = await _userService.UpdateUser(userUpdateDTO);
            if (updatedUser is null)
            {
                _logger.LogWarning("UpdateUser() - No updates made for user: {Email}", userUpdateDTO.Email);
                return NoContent();
            }

            _logger.LogInformation("UpdateUser() - User updated successfully: {Email}", updatedUser.Email);
            return Ok(new { email = updatedUser.Email });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateUser() - Error occurred while updating user: {Email}", userUpdateDTO.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }

    [HttpGet("ConfirmEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        _logger.LogInformation("ConfirmEmail() - Attempting to confirm email for: {Email}", email);
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    _logger.LogInformation("ConfirmEmail() - Email confirmed successfully for: {Email}", email);
                    return View("~/Views/ConfirmationEmail.cshtml");
                }
                else
                {
                    _logger.LogWarning("ConfirmEmail() - Failed to confirm email for: {Email}", email);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Failed to confirm email" });
                }
            }
            else
            {
                _logger.LogWarning("ConfirmEmail() - User not found: {Email}", email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User does not exist" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ConfirmEmail() - Error occurred while confirming email for: {Email}", email);
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "An error occurred while confirming email" });
        }
    }
}
