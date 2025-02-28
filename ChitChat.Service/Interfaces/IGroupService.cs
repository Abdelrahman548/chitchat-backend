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
        Task<BaseResult<PagedList<GroupResponseDto>>> GetAll(ItemQueryParams queryParams, string senderId);
        Task<BaseResult<GroupResponseDto>> GetByID(string groupId, string senderId);
        Task<BaseResult<GroupResponseDto>> Add(GroupRequestDto dto, string senderId);
        Task<BaseResult<string>> UploadPicture(string groupId, IFormFile image, string senderId);
        Task<BaseResult<string>> AddMember(string groupId, string userId, string senderId);
        Task<BaseResult<string>> RemoveMember(string groupId, string memberId, string senderId);
        Task<BaseResult<string>> AddAdmin(string groupId, string memberId, string senderId);
        Task<BaseResult<string>> RemoveAdmin(string groupId, string adminId, string senderId);
        Task<BaseResult<string>> Update(string groupId, GroupRequestDto dto, string adminId);
        //Task<BaseResult<string>> Delete(ObjectId groupId, ObjectId adminId);
        
    }
}
