using ChitChat.Data.Entities.Abstracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Data.Entities
{
    public class RefreshToken : Entity
    {
        [MaxLength(128)]
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }
        // Nav
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;
    }
}
