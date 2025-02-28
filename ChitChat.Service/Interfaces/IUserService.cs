using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IUserService
    {
        Task<BaseResult<PagedList<UserResponseDto>>> GetAll(ItemQueryParams queryParams, string senderId);
        Task<BaseResult<UserResponseDto>> GetByID(string userId);
        Task<BaseResult<string>> Update(string userId, UserRequestDto dto, string senderId);
    }
}
