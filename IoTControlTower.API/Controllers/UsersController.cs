using IoTControlTower.API.Models;
using IoTControlTower.Application.DTO;
using IoTControlTower.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("RegisterUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserToken>> Post([FromBody] UserRegisterDTO applicationUserRegisterDTO, string role)
        {
            var result = await _userService.Post(applicationUserRegisterDTO, role);
            if (result)
                return Ok($"User {applicationUserRegisterDTO.Email} was created with success!");
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login attempt.");
                return BadRequest(ModelState);
            }
        }
    }
}
