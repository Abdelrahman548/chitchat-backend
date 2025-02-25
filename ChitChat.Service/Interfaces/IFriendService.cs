using ChitChat.Data.Entities;
using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IFriendService
    {
        Task<BaseResult<PagedList<FriendResponseDto>>> GetAll(ItemQueryParams queryParams, ObjectId senderId);
        Task<BaseResult<FriendResponseDto>> GetByID(ObjectId friendRequestId, ObjectId senderId);
        Task<BaseResult<FriendResponseDto>> Add(FriendRequest dto);
        Task<BaseResult<string>> Delete(ObjectId friendRequestId, ObjectId senderId);
    }
}
