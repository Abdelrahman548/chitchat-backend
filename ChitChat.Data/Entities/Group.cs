using ChitChat.Data.Entities.Abstracts;
using ChitChat.Data.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Data.Entities
{
    public class Group : SearchableEntity
    {
        public string Name { get; set; } = string.Empty;
        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;
        [MaxLength(128)]
        public string PictureUrl { get; set; } = string.Empty;
        public Visability Visability { get; set; }
        public GroupPermissions Permissions { get; set; }

        // Nav
        [BsonRepresentation(BsonType.ObjectId)]
        public ICollection<string> MembersIds { get; set; } = [];
        [BsonRepresentation(BsonType.ObjectId)]
        public ICollection<string> AdminsIds { get; set; } = [];

        public Group()
        {
            StoredSearchable = Name;
        }

        public override void PrepareSearchable()
        {
            StoredSearchable = $"{Name} {Description}";
        }
    }
}
