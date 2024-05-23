using Microsoft.AspNetCore.Mvc;
using IoTControlTower.API.Models;
using Microsoft.AspNetCore.Authorization;
using IoTControlTower.Application.DTO.User;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly ILogger<UsersController> _logger = logger;
        private readonly IUserService _userService = userService;

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

                var result = await _userService.CreateUser(userRegisterDTO, role);
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

        [HttpPut("UpdateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO userUpdateDTO)
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
