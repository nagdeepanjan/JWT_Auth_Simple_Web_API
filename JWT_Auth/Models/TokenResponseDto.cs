using System.ComponentModel.DataAnnotations;

namespace JWT_Auth.Models
{
    public class TokenResponseDto
    {
        [Required]
        public string AccessToken { get; set; }


        [Required]
        public string RefreshToken { get; set; }
    }
}
