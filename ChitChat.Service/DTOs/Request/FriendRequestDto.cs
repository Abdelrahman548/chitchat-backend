using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Request
{
    public class FriendRequestDto
    {
        [Required]
        public ObjectId SenderId { get; set; }
        [Required]
        public ObjectId ReceiverId { get; set; }
    }
}
