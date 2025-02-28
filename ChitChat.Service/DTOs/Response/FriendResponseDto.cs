using MongoDB.Bson;

namespace ChitChat.Service.DTOs.Response
{
    public class FriendResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
    }
}
