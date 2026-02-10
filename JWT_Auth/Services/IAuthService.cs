using JWT_Auth.Entities;
using JWT_Auth.Models;

namespace JWT_Auth.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto requestUserDto);
        Task<TokenResponseDto?> LoginAsync(UserDto requestUserDto);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
