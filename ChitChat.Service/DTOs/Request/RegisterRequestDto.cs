using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Request
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(50), MinLength(10)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@#$!%*?&#^(){}[\\]<>_+=|\\\\~`:;,.\\/-])[A-Za-z\\d@$!%*?&#^(){}[\\]<>_+=|\\\\~`:;,.\\/-]{10,}$")]
        public string Password { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string OTPEmailVerifyCode { get; set; } = string.Empty;
    }
}
