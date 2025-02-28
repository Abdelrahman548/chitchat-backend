using ChitChat.Data.Entities.Abstracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChitChat.Data.Entities
{
    public class FriendRequest: Entity
    {
        // Nav
        [BsonRepresentation(BsonType.ObjectId)]
        public string SenderId { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReceiverId { get; set; } = string.Empty;
    }
}
