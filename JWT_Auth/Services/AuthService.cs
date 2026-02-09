using Azure.Core;
using JWT_Auth.Data;
using JWT_Auth.Entities;
using JWT_Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Auth.Services
{
    public class AuthService(UserDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<string?> LoginAsync(UserDto requestUserDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == requestUserDto.Username);
            if (user is null)
                return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, requestUserDto.Password) ==
                PasswordVerificationResult.Failed) //Hashed password compare
                return null;
            

            string token = CreateToken(user);

            return token;

        }

        public async Task<User?> RegisterAsync(UserDto requestUserDto)
        {

            if (await context.Users.AnyAsync(u => u.Username == requestUserDto.Username))
                return null;

            var user = new User();
            user.Username = requestUserDto.Username;
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, requestUserDto.Password);

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();


            return (user);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Jwt:Issuer"),
                audience: configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwt;

        }
    }
}
