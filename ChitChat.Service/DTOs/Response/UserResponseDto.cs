using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Response
{
    public class UserResponseDto
    {
        public string Name { get; set; } = string.Empty;
        [MaxLength(128)]
        public string About { get; set; } = string.Empty;
        [MaxLength(128)]
        public string PictureUrl { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? LastSeen { get; set; }
        public bool LastSeenVisability { get; set; }
        public bool OnlineVisability { get; set; }

    }
}
