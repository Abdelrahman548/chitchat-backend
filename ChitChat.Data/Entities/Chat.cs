using ChitChat.Data.Entities.Abstracts;
using MongoDB.Bson;

namespace ChitChat.Data.Entities
{
    public class Chat : Entity
    {
        // Nav
        public ObjectId FirstUserId { get; set; }
        public ObjectId SecondUserId { get; set; }
    }
}
