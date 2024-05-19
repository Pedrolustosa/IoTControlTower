using IoTControlTower.API.Models;
using IoTControlTower.Application.DTO;
using Microsoft.AspNetCore.Mvc;
using IoTControlTower.Application.Interface;


namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatesController(IAuthenticateService authentication) : ControllerBase
    {
        private readonly IAuthenticateService _authentication = authentication;

        [HttpPost("Authenticate")]
        public async Task<ActionResult<UserToken>> Authenticate([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var result = await _authentication.Authenticate(loginDTO.UserName, loginDTO.Password);
                if (result)
                {
                    var token = await _authentication.GenerateToken(loginDTO);
                    return new UserToken { Token = token };
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login attempt.");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
