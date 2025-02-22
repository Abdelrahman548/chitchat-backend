using ChitChat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Message> Messages { get; }
        IRepository<Chat> Chats { get; }
        IRepository<Group> Groups { get; }
        IRepository<Status> Statuses { get; }
        IRepository<FriendRequest> FriendRequests { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        IRepository<EmailOTPVerfication> EmailOTPVerfications { get; }
        IRepository<BannedEmail> BannedEmails { get; }
    }
}
