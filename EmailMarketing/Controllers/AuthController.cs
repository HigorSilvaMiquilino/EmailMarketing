using EmailMarketing.Data;
using EmailMarketing.Models;
using EmailMarketing.Servicos.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmailMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AuthService _authService;

        public AuthController(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            AuthService authService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _userManager.FindByNameAsync(login.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "E-mail ou senha incorretos." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Senha, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "E-mail ou senha incorretos." });
            }

            var token = _authService.GenerateJwtToken(user);
        
            return Ok(new { token });
        }
    }
}
