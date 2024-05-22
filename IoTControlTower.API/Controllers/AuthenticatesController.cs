using Microsoft.AspNetCore.Mvc;
using IoTControlTower.API.Models;
using IoTControlTower.Application.DTO;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatesController(IAuthenticateService authentication, ILogger logger) : ControllerBase
    {
        private readonly ILogger _logger = logger;
        private readonly IAuthenticateService _authentication = authentication;

        [HttpPost("Authenticate")]
        public async Task<ActionResult<UserToken>> Authenticate([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation("Authenticate() - Attempting to authenticate user: {UserName}", loginDTO.UserName);
            try
            {
                var result = await _authentication.Authenticate(loginDTO.UserName, loginDTO.Password);
                if (result)
                {
                    _logger.LogInformation("GenerateToken() - Generating token for user: {UserName}", loginDTO.UserName);
                    var token = await _authentication.GenerateToken(loginDTO);
                    return Ok(new UserToken { Token = token });
                }
                else
                {
                    _logger.LogWarning("GenerateToken() - Invalid login attempt for user: {UserName}", loginDTO.UserName);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authenticate() - Error occurred while authenticating user: {UserName}", loginDTO.UserName);
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
