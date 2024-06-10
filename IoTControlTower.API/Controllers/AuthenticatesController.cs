using Microsoft.AspNetCore.Mvc;
using IoTControlTower.API.Models;
using Microsoft.AspNetCore.Authorization;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Users;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatesController(IAuthenticateService authenticationService, 
                                         ILogger<AuthenticatesController> logger) : ControllerBase
    {
        private readonly ILogger<AuthenticatesController> _logger = logger;
        private readonly IAuthenticateService _authenticationService = authenticationService;

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> Authenticate([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation("Authenticate() - Attempting to authenticate user: {Email}", loginDTO.Email);
            try
            {
                var result = await _authenticationService.Authenticate(loginDTO);
                if (result)
                {
                    _logger.LogInformation("GenerateToken() - Generating token for user: {Email}", loginDTO.Email);
                    var token = await _authenticationService.GenerateToken(loginDTO);
                    _logger.LogInformation("Authenticate() - Token generated successfully for user: {Email}", loginDTO.Email);
                    return Ok(new UserToken { Token = token });
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt for user: {Email}", loginDTO.Email);
                    return BadRequest("Invalid login attempt.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authenticate() - Error occurred while authenticating user: {Email}", loginDTO.Email);
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
