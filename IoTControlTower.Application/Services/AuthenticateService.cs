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

namespace IoTControlTower.Application.Service;

public class AuthenticateService(SignInManager<User> signInManager, 
                                 UserManager<User> userManager, 
                                 IUserService userService,
                                 IConfiguration configuration,
                                 ILogger<AuthenticateService>  logger) : IAuthenticateService
{
    private readonly IUserService _userService = userService;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ILogger<AuthenticateService> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly SignInManager<User> _signInManager = signInManager;

    public async Task<bool> Authenticate(LoginDTO loginDTO)
    {
        _logger.LogInformation("Authenticate() - Attempting to authenticate user: {Email}", loginDTO.Email);
        try
        {
            var user = await _userService.GetUserByEmail(loginDTO.Email);
            if (user is null)
            {
                _logger.LogWarning("Authenticate() - User does not exist: {Email}", loginDTO.Email);
                throw new Exception("User does not exist!");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDTO.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("Authenticate() - User authenticated successfully: {Email}", loginDTO.Email);
                if (user is not null)
                {
                    user.Password = loginDTO.Password;
                    await _userService.UpdateUser(user);
                }
                return true;
            }
            else
            {
                _logger.LogWarning("Authenticate() - Invalid login attempt for user: {Email}", loginDTO.Email);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Authenticate() - Error occurred while authenticating user: {Email}", loginDTO.Email);
            throw;
        }
    }

    public async Task<string> GenerateToken(LoginDTO loginDTO)
    {
        _logger.LogInformation("GenerateToken() - Generating token for user: {Email}", loginDTO.Email);
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null)
            {
                _logger.LogWarning("GenerateToken() - User not found: {Email}", loginDTO.Email);
                throw new Exception($"User not found: {loginDTO.Email}");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, loginDTO.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);
            var expirationTime = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expirationTime,
                signingCredentials: credentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("GenerateToken() - Token generated successfully for user: {Email}", loginDTO.Email);
            return tokenString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GenerateToken() - Error occurred while generating token for user: {Email}", loginDTO.Email);
            throw;
        }
    }
}
