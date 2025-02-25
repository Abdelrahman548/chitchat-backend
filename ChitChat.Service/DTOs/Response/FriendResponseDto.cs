using MongoDB.Bson;

namespace ChitChat.Service.DTOs.Response
{
    public class FriendResponseDto
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
    }
}
