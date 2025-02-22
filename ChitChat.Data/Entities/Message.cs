using ChitChat.Data.Entities.Abstracts;
using ChitChat.Data.Helpers;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

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
        public ObjectId SenderId { get; set; }
        public ObjectId ReceiverId { get; set; }
        public ObjectId ChatId { get; set; }

        public Message()
        {
            StoredSearchable = Text;
        }
    }
}
