using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IChatMessageService
    {   
        Task<BaseResult<PagedList<MessageResponseDto>>> GetAll(string chatId, ItemQueryParams queryParams, string memberId);
        Task<BaseResult<MessageResponseDto>> GetByID(string messageId, string memberId);
        Task<BaseResult<MessageResponseDto>> Add(MessageRequestDto dto, string senderId);
        Task<BaseResult<MessageResponseDto>> Update(string messageId, MessageRequestDto dto, string senderId);
        Task<BaseResult<string>> Delete(string messageId, string senderId);
    }
}
