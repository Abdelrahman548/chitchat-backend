using ChitChat.Data.Entities.Abstracts;
using ChitChat.Data.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Data.Entities
{
    public class Status : Entity
    {
        [MaxLength(128)]
        public string Text { get; set; } = string.Empty;
        public string PayloadUrl { get; set; } = string.Empty;
        public ContentType ContentType { get; set; }
        public DateTime ExpirationTime { get; set; }

        // Nav
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

    }
}
