using System.ComponentModel.DataAnnotations;

namespace JWT_Auth.Models
{
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
