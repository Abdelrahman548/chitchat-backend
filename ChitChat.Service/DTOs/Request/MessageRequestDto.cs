using ChitChat.Data.Helpers;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Request
{
    public class MessageRequestDto
    {
        [Required]
        [MaxLength(128)]
        public string Text { get; set; } = string.Empty;

        public IFormFile? Payload { get; set; }
        [Required]
        public ContentType ContentType { get; set; }

        // Nav
        [Required]
        public string ReceiverId { get; set; } = string.Empty;
    }
}
