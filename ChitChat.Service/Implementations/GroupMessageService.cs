using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;

namespace ChitChat.Service.Implementations
{
    public class GroupMessageService : IGroupMessageService
    {
        public Task<BaseResult<MessageResponseDto>> Add(ObjectId groupId, MessageRequestDto dto, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<string>> Delete(ObjectId groupId, ObjectId messageId, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<PagedList<MessageResponseDto>>> GetAll(ObjectId groupId, ItemQueryParams queryParams, ObjectId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<MessageResponseDto>> GetByID(ObjectId messageId, ObjectId memberId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<MessageResponseDto>> Update(ObjectId groupId, MessageRequestDto dto, ObjectId senderId)
        {
            throw new NotImplementedException();
        }
    }
}
