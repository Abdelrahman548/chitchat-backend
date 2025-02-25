using ChitChat.Data.Entities.Abstracts;
using MongoDB.Bson;

namespace ChitChat.Data.Entities
{
    public class FriendRequest
    {
        // Nav
        public ObjectId SenderId { get; set; }
        public ObjectId ReceiverId { get; set; }
    }
}
