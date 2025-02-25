using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Request
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(128)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;
    }
}
