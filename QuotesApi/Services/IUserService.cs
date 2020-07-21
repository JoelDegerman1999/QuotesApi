using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuotesApi.Models;
using QuotesApi.Models.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApi.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);

        Task<UserManagerResponse> RegisterAdminAsync(RegisterViewModel model);

        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);

    }

    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _iConfiguration;

        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration iConfiguration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _iConfiguration = iConfiguration;
        }


        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Reigster Model is null");

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };

            var identityUser = new AppUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName

            };
            var result = await _userManager.CreateAsync(identityUser, model.Password);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var userRole = await _roleManager.FindByNameAsync("User");
                if (userRole == null)
                {
                    userRole = new IdentityRole("User");
                    await _roleManager.CreateAsync(userRole);
                }


                if (!await _userManager.IsInRoleAsync(user, userRole.Name))
                {
                    await _userManager.AddToRoleAsync(user, userRole.Name);
                }
            }


            if (result.Succeeded)
            {
                return new UserManagerResponse()
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(s => s.Description)
            };
        }

        public async Task<UserManagerResponse> RegisterAdminAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Register Model is null");

            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };

            var identityUser = new AppUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(identityUser, model.Password);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var adminRole = await _roleManager.FindByNameAsync("Admin");
                if (adminRole == null)
                {
                    adminRole = new IdentityRole("Admin");
                    await _roleManager.CreateAsync(adminRole);
                }

                if (!await _userManager.IsInRoleAsync(user, adminRole.Name))
                {
                    await _userManager.AddToRoleAsync(user, adminRole.Name);
                }
            }


            if (result.Succeeded)
            {
                return new UserManagerResponse()
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(s => s.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid email or password",
                    IsSuccess = false,
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new UserManagerResponse
                {
                    Message = "Invalid email or password",
                    IsSuccess = false,
                };

            var role = await _userManager.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, role[0])
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iConfiguration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _iConfiguration["AuthSettings:Issuer"],
                audience: _iConfiguration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);



            var userView = new AppUserViewModel
            {
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName
            };

            return new UserManagerResponse
            {
                Token = tokenAsString,
                AppUser = userView,
                IsSuccess = true,
                TokenExpireDate = token.ValidTo
            };
        }
    }
}