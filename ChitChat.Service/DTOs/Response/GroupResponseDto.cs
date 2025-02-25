using ChitChat.Data.Entities.Abstracts;
using ChitChat.Data.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.DTOs.Response
{
    public class GroupResponseDto : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public Visability Visability { get; set; }
        public GroupPermissions Permissions { get; set; }
    }
}
