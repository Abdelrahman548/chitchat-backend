using ChitChat.Data.Entities.Abstracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChitChat.Data.Entities
{
    public class Chat : Entity
    {
        // Nav
        [BsonRepresentation(BsonType.ObjectId)]
        public string FirstUserId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string SecondUserId { get; set; } = string.Empty;
    }
}
