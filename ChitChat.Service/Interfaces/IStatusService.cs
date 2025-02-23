using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IStatusService
    {
        Task<BaseResult<PagedList<StatusResponseDto>>> GetAll(ObjectId userId, ItemQueryParams queryParams, ObjectId senderId);
        Task<BaseResult<StatusResponseDto>> GetByID(ObjectId statusId, ObjectId senderId);
        Task<BaseResult<StatusResponseDto>> Add(ObjectId userId, StatusRequestDto dto);
        Task<BaseResult<string>> Delete(ObjectId statusId, ObjectId senderId);
    }
}
