using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChitChat.Data.Entities.Abstracts
{
    public abstract class Entity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
