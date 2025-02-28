using ChitChat.Data.Entities.Abstracts;
using ChitChat.Data.Helpers;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Response
{
    public class StatusResponseDto : Entity
    {
        public string Text { get; set; } = string.Empty;
        public string PayloadUrl { get; set; } = string.Empty;
        public ContentType ContentType { get; set; }
        public DateTime ExpirationTime { get; set; }

        // Nav
        public string UserId { get; set; } = string.Empty;
    }
}
