using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;

namespace ChitChat.Service.Implementations
{
    public class GroupService : IGroupService
    {
        public Task<BaseResult<GroupResponseDto>> Add(GroupRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<string>> Delete(ObjectId groupId, ObjectId adminId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<PagedList<GroupResponseDto>>> GetAll(ItemQueryParams queryParams, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<GroupResponseDto>> GetByID(ObjectId groupId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<ObjectId>> Update(ObjectId groupId, GroupRequestDto dto, ObjectId adminId)
        {
            throw new NotImplementedException();
        }
    }
}
