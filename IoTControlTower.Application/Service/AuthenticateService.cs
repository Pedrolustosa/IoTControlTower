using AutoMapper;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using IoTControlTower.Domain.Entities;
using Microsoft.Extensions.Configuration;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Users;

namespace IoTControlTower.Application.Service
{
    public class AuthenticateService(SignInManager<User> signInManager, 
                                     UserManager<User> userManager, 
                                     IUserService userService, 
                                     IMapper mapper, 
                                     IConfiguration configuration,
                                     ILogger<AuthenticateService>  logger) : IAuthenticateService
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly ILogger<AuthenticateService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> Authenticate(LoginDTO loginDTO)
        {
            _logger.LogInformation("Authenticate() - Attempting to authenticate user: {UserName}", loginDTO.UserName);

            try
            {
                var existUser = await _userService.GetUserName(loginDTO.UserName);

                if (!existUser)
                {
                    _logger.LogWarning("Authenticate() - User does not exist: {UserName}", loginDTO.UserName);
                    throw new Exception("User does not exist!");
                }

                var result = await _signInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Authenticate() - User authenticated successfully: {UserName}", loginDTO.UserName);
                    var user = await _userService.GetUserData(new AuthenticateDTO { UserName = loginDTO.UserName });
                    if (user != null)
                    {
                        var userUpdateDTO = _mapper.Map<UserUpdateDTO>(user);
                        await _userService.UpdateUser(userUpdateDTO);
                    }
                    return true;
                }
                else
                {
                    _logger.LogWarning("Authenticate() - Invalid login attempt for user: {UserName}", loginDTO.UserName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authenticate() - Error occurred while authenticating user: {UserName}", loginDTO.UserName);
                throw;
            }
        }

        public async Task<string> GenerateToken(LoginDTO loginDTO)
        {
            _logger.LogInformation("GenerateToken() - Generating token for user: {UserName}", loginDTO.UserName);

            try
            {
                var user = await _userManager.FindByNameAsync(loginDTO.UserName);

                if (user is null)
                {
                    _logger.LogWarning("GenerateToken() - User not found: {UserName}", loginDTO.UserName);
                    throw new Exception($"User not found: {loginDTO.UserName}");
                }

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
                var expirationTime = DateTime.UtcNow.AddMinutes(10);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: expirationTime,
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("GenerateToken() - Token generated successfully for user: {UserName}", loginDTO.UserName);
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GenerateToken() - Error occurred while generating token for user: {UserName}", loginDTO.UserName);
                throw;
            }
        }
    }
}
