using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IUserService
    {
        Task<BaseResult<PagedList<UserResponseDto>>> GetAll(ItemQueryParams queryParams, ObjectId senderId);
        Task<BaseResult<UserResponseDto>> GetByID(ObjectId userId);
        Task<BaseResult<ObjectId>> Update(ObjectId userId, UserRequestDto dto, ObjectId senderId);
    }
}
