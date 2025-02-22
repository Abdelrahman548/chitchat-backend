using ChitChat.Data.Entities.Abstracts;
using MongoDB.Bson;
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
        public ObjectId RefreshTokenId { get; set; }
        public ICollection<ObjectId> GroupsIds { get; set; } = [];
        public ICollection<ObjectId> StatusIds { get; set; } = [];
        public ICollection<ObjectId> ChatsIds { get; set; } = [];
        public ICollection<ObjectId> BlockedUsersIds { get; set; } = [];
        public ICollection<ObjectId> FriendRequestsIds { get; set; } = [];

        public User()
        {
            StoredSearchable = Name;
        }
    }
}
