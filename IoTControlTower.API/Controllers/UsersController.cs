using IoTControlTower.API.Models;
using IoTControlTower.Application.DTO;
using IoTControlTower.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("RegisterUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserToken>> Post([FromBody] UserRegisterDTO userRegisterDTO, string role)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userService.Post(userRegisterDTO, role);
                if (result)
                    return Ok($"User {userRegisterDTO.Email} was created with success!");
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
