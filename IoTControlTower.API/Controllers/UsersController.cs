using Microsoft.AspNetCore.Mvc;
using IoTControlTower.API.Models;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using IoTControlTower.Application.DTO.User;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, 
                                 ILogger<UsersController> logger, 
                                 UserManager<User> userManager) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<UsersController> _logger = logger;
        private readonly UserManager<User> _userManager = userManager;

        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserRegisterDTO userRegisterDTO, string role)
        {
            _logger.LogInformation("CreateUser() - Registering a new user with email: {Email} and role: {Role}", userRegisterDTO.Email, role);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("CreateUser() - Model state is invalid for user registration.");
                    return BadRequest(ModelState);
                }

                var urlHelper = Url;
                var scheme = Request.Scheme;
                var result = await _userService.CreateUser(userRegisterDTO, role, urlHelper, scheme);
                if (result)
                {
                    _logger.LogInformation("CreateUser() - User {Email} was created successfully.", userRegisterDTO.Email);
                    return Ok($"User {userRegisterDTO.Email} was created with success!");
                }
                else
                {
                    _logger.LogWarning("CreateUser() - Error creating user {Email}.", userRegisterDTO.Email);
                    ModelState.AddModelError(string.Empty, "Error creating user.");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUser() - Error occurred while creating user {Email}.", userRegisterDTO.Email);
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {ex.Message}");
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

                if (applicationUserUpdate == null)
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
    }
}
