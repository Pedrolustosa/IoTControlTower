using AutoMapper;
using IoTControlTower.Application.DTO;
using IoTControlTower.Application.Interface;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IoTControlTower.Application.Service
{
    public class AuthenticateService(SignInManager<User> signInManager, UserManager<User> userManager, IUserRepository userRepository, IConfiguration configuration) : IAuthenticateService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<bool> Authenticate(string userName, string password)
        {
            try
            {
                var statusUser = await _userRepository.GetUserName(userName);
                if (statusUser)
                {
                    var result = await _signInManager.PasswordSignInAsync(userName, password, false, lockoutOnFailure: false);
                    return result.Succeeded;
                }
                throw new Exception("User Inactive!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GenerateToken(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDTO.UserName);
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, loginDTO.UserName),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);
                var expirations = DateTime.UtcNow.AddMinutes(7);

                JwtSecurityToken token = new
                (
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: expirations,
                    signingCredentials: credentials
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
