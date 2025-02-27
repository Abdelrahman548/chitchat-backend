using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IGroupService
    {
        Task<BaseResult<PagedList<GroupResponseDto>>> GetAll(ItemQueryParams queryParams, ObjectId senderId);
        Task<BaseResult<GroupResponseDto>> GetByID(ObjectId groupId, ObjectId senderId);
        Task<BaseResult<GroupResponseDto>> Add(GroupRequestDto dto, ObjectId senderId);
        Task<BaseResult<string>> UploadPicture(ObjectId groupId, IFormFile image, ObjectId senderId);
        Task<BaseResult<string>> AddMember(ObjectId groupId, ObjectId userId, ObjectId senderId);
        Task<BaseResult<string>> RemoveMember(ObjectId groupId, ObjectId memberId, ObjectId senderId);
        Task<BaseResult<string>> AddAdmin(ObjectId groupId, ObjectId memberId, ObjectId senderId);
        Task<BaseResult<string>> RemoveAdmin(ObjectId groupId, ObjectId adminId, ObjectId senderId);
        Task<BaseResult<string>> Update(ObjectId groupId, GroupRequestDto dto, ObjectId adminId);
        //Task<BaseResult<string>> Delete(ObjectId groupId, ObjectId adminId);
        
    }
}
