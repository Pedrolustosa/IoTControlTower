using Microsoft.AspNetCore.Mvc;
using IoTControlTower.API.Models;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Users;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatesController(IAuthenticateService authenticationService, ILogger<AuthenticatesController> logger) : ControllerBase
    {
        
        private readonly ILogger<AuthenticatesController> _logger = logger;
        private readonly IAuthenticateService _authenticationService = authenticationService;

        [HttpPost("Authenticate")]
        public async Task<ActionResult<UserToken>> Authenticate([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation("Authenticate() - Attempting to authenticate user: {UserName}", loginDTO.UserName);
            try
            {
                var result = await _authenticationService.Authenticate(loginDTO);
                if (result)
                {
                    _logger.LogInformation("GenerateToken() - Generating token for user: {UserName}", loginDTO.UserName);
                    var token = await _authenticationService.GenerateToken(loginDTO);
                    _logger.LogInformation("Authenticate() - Token generated successfully for user: {UserName}", loginDTO.UserName);
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
