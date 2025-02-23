using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IGroupMessageService
    {
        Task<BaseResult<PagedList<MessageResponseDto>>> GetAll(ObjectId groupId, ItemQueryParams queryParams, ObjectId memberId);
        Task<BaseResult<MessageResponseDto>> GetByID(ObjectId messageId, ObjectId memberId);
        Task<BaseResult<MessageResponseDto>> Add(ObjectId groupId, MessageRequestDto dto, ObjectId senderId);
        Task<BaseResult<MessageResponseDto>> Update(ObjectId groupId, MessageRequestDto dto, ObjectId senderId);
        Task<BaseResult<string>> Delete(ObjectId groupId, ObjectId messageId, ObjectId senderId);
    }
}
