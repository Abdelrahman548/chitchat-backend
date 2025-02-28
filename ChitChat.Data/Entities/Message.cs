using ChitChat.Data.Entities.Abstracts;
using ChitChat.Data.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ChitChat.Data.Entities
{
    public class Message : SearchableEntity
    {
        [MaxLength(128)]
        public string Text { get; set; } = string.Empty;
        public string PayloadUrl { get; set; } = string.Empty;
        public ChatType ChatType { get; set; }
        public ContentType ContentType { get; set; }

        // Nav
        [BsonRepresentation(BsonType.ObjectId)]
        public string SenderId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string ReceiverId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)] 
        public string ChatId { get; set; } = string.Empty;

        public Message()
        {
            StoredSearchable = Text;
        }
        public override void PrepareSearchable()
        {
            StoredSearchable = Text;
        }
    }
}
