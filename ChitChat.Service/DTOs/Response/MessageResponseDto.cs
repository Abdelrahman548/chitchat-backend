using ChitChat.Data.Entities.Abstracts;
using ChitChat.Data.Helpers;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Response
{
    public class MessageResponseDto : Entity
    {
        public string Text { get; set; } = string.Empty;
        public string PayloadUrl { get; set; } = string.Empty;
        public ContentType ContentType { get; set; }

        // Nav
        public string SenderId { get; set; } = string.Empty;
        public string ChatId { get; set; } = string.Empty;
    }
}
