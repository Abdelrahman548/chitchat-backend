using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Request
{
    public class VerifyEmailRequestDto
    {
        [EmailAddress]
        [MaxLength(128)]
        public string Email { get; set; } = string.Empty;
    }
}
