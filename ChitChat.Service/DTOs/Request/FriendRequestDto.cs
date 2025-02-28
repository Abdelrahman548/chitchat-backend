using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Request
{
    public class FriendRequestDto
    {
        [Required]
        public string SenderId { get; set; } = string.Empty;
        [Required]
        public string ReceiverId { get; set; } = string.Empty;
    }
}
