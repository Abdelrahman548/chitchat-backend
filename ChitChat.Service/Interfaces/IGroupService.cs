using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IGroupService
    {
        Task<BaseResult<PagedList<GroupResponseDto>>> GetAll(ItemQueryParams queryParams, ObjectId senderId);
        Task<BaseResult<GroupResponseDto>> GetByID(ObjectId groupId);
        Task<BaseResult<GroupResponseDto>> Add(GroupRequestDto dto);
        Task<BaseResult<ObjectId>> Update(ObjectId groupId, GroupRequestDto dto, ObjectId adminId);
        Task<BaseResult<string>> Delete(ObjectId groupId, ObjectId adminId);
        
    }
}
