using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;

namespace ChitChat.Service.Implementations
{
    public class StatusService : IStatusService
    {
        public Task<BaseResult<StatusResponseDto>> Add(ObjectId userId, StatusRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<string>> Delete(ObjectId statusId, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<PagedList<StatusResponseDto>>> GetAll(ObjectId userId, ItemQueryParams queryParams, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<StatusResponseDto>> GetByID(ObjectId statusId, ObjectId senderId)
        {
            throw new NotImplementedException();
        }
    }
}
