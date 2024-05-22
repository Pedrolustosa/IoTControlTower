using AutoMapper;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Application.DTO;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.Application.Service
{
    public class UserService(UserManager<User> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IUserRepository userRepository,
                                        IMapper mapper) : IUserService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        

        public async Task<bool> CreateUser(UserRegisterDTO userRegisterDTO, string role)
        {
            try
            {
                var hasUser = await GetUserName(userRegisterDTO.UserName);
                if (hasUser)
                    throw new Exception("User already exists.");

                var user = _mapper.Map<User>(userRegisterDTO);
                if (user is not null)
                {
                    var userExist = await _userManager.FindByEmailAsync(user.Email);
                    if (userExist != null)
                        throw new Exception("Email already exists.");

                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        var result = await _userManager.CreateAsync(user, userRegisterDTO.Password);
                        if (!result.Succeeded)
                            throw new Exception("Failed to create user.");

                        await _userManager.AddToRoleAsync(user, role);
                        return true;
                    }
                    else
                    {
                        throw new Exception("Please choose a valid role for this user.");
                    }
                }
                else
                {
                    throw new Exception("Invalid user data.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> GetUserName(string userName)
        {
            try
            {
                var hasUser = await _userRepository.GetUserName(userName);
                return hasUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetUserId()
        {
            try
            {
                return await _userRepository.GetUserId();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
