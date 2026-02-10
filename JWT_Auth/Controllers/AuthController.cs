using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT_Auth.Entities;
using JWT_Auth.Models;
using JWT_Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        public static User user = new();
        
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto requestUserDto)
        {

            var user = await authService.RegisterAsync(requestUserDto);
            if (user is null)
                return BadRequest("Username already exists");


            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto requestUserDto)
        {
            var result = await authService.LoginAsync(requestUserDto);

            if (result is null)
                return BadRequest("Invalid username or password");
            
            return Ok(result);
        }


        [HttpGet]
        [Authorize]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [HttpGet("admin-only")]
        [Authorize(Roles="Admin")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are authenticated and an ADMIN!");
        }
    }
}
