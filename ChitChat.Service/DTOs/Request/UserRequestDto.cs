using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Request
{
    public class UserRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(128)]
        public string About { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public bool LastSeenVisability { get; set; }
        [Required]
        public bool OnlineVisability { get; set; }
    }
}
