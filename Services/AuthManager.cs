using AutoMapper;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;

namespace Services
{
        public class AuthManager : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        
        private readonly IMapper _mapper;

        public AuthManager(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public IEnumerable<IdentityRole> Roles => _roleManager.Roles;

        public async Task<IdentityResult> CreateUser(UserDtoForCreation userDto)
        {
            var user = _mapper.Map<IdentityUser>(userDto);
            var result = await _userManager.CreateAsync(user,userDto.Password);

            if(!result.Succeeded)
                throw new Exception("Cannot create user!");

            if(userDto.Roles.Count>0)
            {
                var roleResult = await _userManager.AddToRolesAsync(user,userDto.Roles);
                if(!roleResult.Succeeded)
                    throw new Exception("Error!!");
                
            }

            return result;
        }

        public IEnumerable<IdentityUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<IdentityUser> GetOneUser(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<UserDtoForUpdate> GetOneUserForUpdate(string userName)
        {
            var user = await GetOneUser(userName);
            if(user is not null)
            {
                var userDto = _mapper.Map<UserDtoForUpdate>(user);
                userDto.Roles =  new HashSet<string>(Roles.Select( r=> r.Name).ToList());
                userDto.UserRoles = new HashSet<string>(await _userManager.GetRolesAsync(user));
                return userDto;
            }
            throw new Exception("cannot get user");
        }

        public async Task UpdateUser(UserDtoForUpdate userDto)
        {
            var user = await GetOneUser(userDto.Username);
            user.PhoneNumber = userDto.PhoneNumber;
            user.Email = userDto.Email;

            if(user is not null)
            {
                var result = await _userManager.UpdateAsync(user);
                if(userDto.Roles.Count > 0)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user,userRoles);
                    await _userManager.AddToRolesAsync(user,userDto.Roles);
                }
            }
        }
    }
}