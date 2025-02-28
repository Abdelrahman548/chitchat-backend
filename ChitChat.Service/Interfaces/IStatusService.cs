using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IStatusService
    {
        Task<BaseResult<PagedList<StatusResponseDto>>> GetAll(string userId, ItemQueryParams queryParams, string senderId);
        Task<BaseResult<StatusResponseDto>> GetByID(string statusId, string senderId);
        Task<BaseResult<StatusResponseDto>> Add(string userId, StatusRequestDto dto);
        Task<BaseResult<string>> Delete(string statusId, string senderId);
    }
}
