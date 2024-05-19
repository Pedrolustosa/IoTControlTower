using AutoMapper;
using IoTControlTower.Application.DTO;
using IoTControlTower.Application.Interface;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface;
using Microsoft.AspNetCore.Identity;

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

        public async Task<bool> Post(UserRegisterDTO userRegisterDTO, string role)
        {
            var hasUser = GetUserName(userRegisterDTO.UserName);
            if(hasUser.Result)
                throw new Exception("Have this User");

            var user = _mapper.Map<User>(userRegisterDTO);
            if (user is not null)
            {
                var userExist = await _userManager.FindByEmailAsync(user.Email);
                if (userExist != null) throw new Exception("Email already exists.");
                if (await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _userManager.CreateAsync(user, userRegisterDTO.Password);
                    if (!result.Succeeded) throw new Exception("Error!");
                    await _userManager.AddToRoleAsync(user, role);
                    return result.Succeeded;
                }
                else
                    throw new Exception("Please, choose a role for this user!");
            }
            else
            {
                throw new Exception("CPF or Age Invalid");
            }
        }

        public async Task<bool> GetUserName(string userName)
        {
            var hasUser = await _userRepository.GetUserName(userName);
            return hasUser;
        }
    }
}
