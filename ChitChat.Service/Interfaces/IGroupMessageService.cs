using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IGroupMessageService
    {
        Task<BaseResult<PagedList<MessageResponseDto>>> GetAll(string groupId, ItemQueryParams queryParams, string memberId);
        Task<BaseResult<MessageResponseDto>> GetByID(string messageId, string memberId);
        Task<BaseResult<MessageResponseDto>> Add(string groupId, MessageRequestDto dto, string senderId);
        Task<BaseResult<MessageResponseDto>> Update(string messageId, MessageRequestDto dto, string senderId);
        Task<BaseResult<string>> Delete(string groupId, string messageId, string senderId);
    }
}
