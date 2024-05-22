using Microsoft.AspNetCore.Mvc;
using IoTControlTower.API.Models;
using IoTControlTower.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, ILogger logger) : ControllerBase
    {
        private readonly ILogger _logger = logger;
        private readonly IUserService _userService = userService;

        [HttpPost("RegisterUser")]
        [Authorize(Roles = "Admin")]
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
                _logger.LogError(ex, "Post() - Error occurred while creating user {Email}.", userRegisterDTO.Email);
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro: {ex.Message}");
            }
        }
    }
}
