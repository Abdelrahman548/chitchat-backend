using ChitChat.Data.Helpers;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;


namespace ChitChat.Service.DTOs.Request
{
    public class GroupRequestDto
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public Visability Visability { get; set; }
        [Required]
        public GroupPermissions Permissions { get; set; }

    }
}
