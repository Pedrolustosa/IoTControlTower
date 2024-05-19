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
    public class AuthenticateService(SignInManager<User> signInManager, IUserRepository userRepository, IConfiguration configuration) : IAuthenticateService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<bool> Authenticate(string userName, string password)
        {
            var statusUser = await _userRepository.GetUserName(userName);
            if (statusUser)
            {
                var result = await _signInManager.PasswordSignInAsync(userName, password, false, lockoutOnFailure: false);
                return result.Succeeded;
            }
            throw new Exception("User Inactive!");
        }

        public string GenerateToken(string userName)
        {
            var claims = new[]
            {
                new Claim("email", userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

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
    }
}
