using ChitChat.Data.Entities;
using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IFriendService
    {
        Task<BaseResult<PagedList<FriendResponseDto>>> GetAll(ItemQueryParams queryParams, string senderId);
        Task<BaseResult<FriendResponseDto>> GetByID(string friendRequestId, string senderId);
        Task<BaseResult<string>> Add(FriendRequestDto dto);
        Task<BaseResult<string>> Cancel(string friendRequestId, string senderId);
        Task<BaseResult<string>> Accept(string friendRequestId, string senderId);
    }
}
