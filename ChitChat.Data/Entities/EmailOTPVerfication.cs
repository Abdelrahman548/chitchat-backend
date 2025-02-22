using ChitChat.Data.Entities.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Data.Entities
{
    public class EmailOTPVerfication : Entity
    {
        [EmailAddress]
        [MaxLength(128)]
        public string Email { get; set; } = string.Empty;
        public string HashedOTP { get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; }
    }
}
