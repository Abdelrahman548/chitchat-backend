using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;

namespace ChitChat.Service.Implementations
{
    public class ChatMessageService : IChatMessageService
    {
        public Task<BaseResult<MessageResponseDto>> Add(ObjectId chatId, MessageRequestDto dto, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<string>> Delete(ObjectId chatId, ObjectId messageId, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<PagedList<MessageResponseDto>>> GetAll(ObjectId chatId, ItemQueryParams queryParams, ObjectId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<MessageResponseDto>> GetByID(ObjectId messageId, ObjectId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<MessageResponseDto>> Update(ObjectId chatId, MessageRequestDto dto, ObjectId senderId)
        {
            throw new NotImplementedException();
        }
    }
}
