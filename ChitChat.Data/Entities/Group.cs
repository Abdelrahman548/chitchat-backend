using ChitChat.Data.Entities.Abstracts;
using ChitChat.Data.Helpers;
using MongoDB.Bson;
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
        public ICollection<ObjectId> MembersIds { get; set; } = [];
        public ICollection<ObjectId> AdminsIds { get; set; } = [];

        public Group()
        {
            StoredSearchable = Name;
        }
    }
}
