using ChitChat.Data.Entities;
using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;

namespace ChitChat.Service.Implementations
{
    public class FriendService : IFriendService
    {
        public Task<BaseResult<FriendResponseDto>> Add(FriendRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<string>> Delete(ObjectId friendRequestId, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<PagedList<FriendResponseDto>>> GetAll(ItemQueryParams queryParams, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<FriendResponseDto>> GetByID(ObjectId friendRequestId, ObjectId senderId)
        {
            throw new NotImplementedException();
        }
    }
}
