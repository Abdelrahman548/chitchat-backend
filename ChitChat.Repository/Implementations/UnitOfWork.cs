using ChitChat.Data.Entities;
using ChitChat.Repository.Interfaces;
using MongoDB.Driver;

namespace ChitChat.Repository.Implementations
{
    public class UnitOfWork: IUnitOfWork
    {
        public UnitOfWork(IMongoDatabase mongoClient)
        {
            Users = new Repository<User>(mongoClient, "Users");
            Messages = new Repository<Message>(mongoClient, "Messages");
            Chats = new Repository<Chat>(mongoClient, "Chats");
            Groups = new Repository<Group>(mongoClient, "Groups");
            Statuses = new Repository<Status>(mongoClient, "Statuses");
            FriendRequests = new Repository<FriendRequest>(mongoClient, "FriendRequests");
            RefreshTokens = new Repository<RefreshToken>(mongoClient, "RefreshTokens");
            EmailOTPVerfications = new Repository<EmailOTPVerfication>(mongoClient, "EmailOTPVerfications");
            BannedEmails = new Repository<BannedEmail>(mongoClient, "BannedEmails");
        }
        public IRepository<User> Users { get; }

        public IRepository<Message> Messages { get; }

        public IRepository<Chat> Chats { get; }

        public IRepository<Group> Groups { get; }

        public IRepository<Status> Statuses { get; }

        public IRepository<FriendRequest> FriendRequests { get; }

        public IRepository<RefreshToken> RefreshTokens { get; }

        public IRepository<EmailOTPVerfication> EmailOTPVerfications { get; }

        public IRepository<BannedEmail> BannedEmails { get; }
    }
}
