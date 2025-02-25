using ChitChat.Data.Helpers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Service.DTOs.Request
{
    public class StatusRequestDto
    {
        [Required]
        [MaxLength(128)]
        public string Text { get; set; } = string.Empty;

        public IFormFile? Payload{ get; set; }
        [Required]
        public ContentType ContentType { get; set; }
        [Required]
        public DateTime ExpirationTime { get; set; }
    }
}
