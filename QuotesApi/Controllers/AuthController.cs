using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Models.Auth;
using QuotesApi.Services;
using System.Threading.Tasks;

namespace QuotesApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await _userService.RegisterUserAsync(model);

            if (result.IsSuccess) return Ok(result);

            return BadRequest(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel model)
        {
            var result = await _userService.RegisterAdminAsync(model);
            if (result.IsSuccess) return Ok(result);

            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var result = await _userService.LoginUserAsync(model);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
