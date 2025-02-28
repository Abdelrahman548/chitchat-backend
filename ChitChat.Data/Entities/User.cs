using ChitChat.Data.Entities.Abstracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ChitChat.Data.Entities
{
    public class User : SearchableEntity
    {
        [EmailAddress]
        [MaxLength(128)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(256)]
        public string Password { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(128)]
        public string About { get; set; } = string.Empty;
        [MaxLength(128)]
        public string PictureUrl { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? LastSeen { get; set; }
        public bool LastSeenVisability { get; set; }
        public bool OnlineVisability { get; set; }

        public DateTime? BanUntil { get; set; }
        public int NumOfBans { get; set; }

        // Nav
        [BsonRepresentation(BsonType.ObjectId)]
        public string RefreshTokenId { get; set; } = string.Empty;
        
        [BsonRepresentation(BsonType.ObjectId)]
        public ICollection<string> GroupsIds { get; set; } = [];
        
        [BsonRepresentation(BsonType.ObjectId)] 
        public ICollection<string> StatusIds { get; set; } = [];
        
        [BsonRepresentation(BsonType.ObjectId)] 
        public ICollection<string> ChatsIds { get; set; } = [];

        [BsonRepresentation(BsonType.ObjectId)]
        public ICollection<string> BlockedUsersIds { get; set; } = [];

        [BsonRepresentation(BsonType.ObjectId)]
        public ICollection<string> FriendRequestsIds { get; set; } = [];
        
        [BsonRepresentation(BsonType.ObjectId)]
        public ICollection<string> FriendsIds { get; set; } = [];

        public User()
        {
            StoredSearchable = Name;
        }

        public override void PrepareSearchable()
        {
            StoredSearchable = Name;
        }
    }
}
