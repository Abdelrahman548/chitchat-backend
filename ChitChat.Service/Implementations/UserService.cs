using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;

namespace ChitChat.Service.Implementations
{
    public class UserService : IUserService
    {
        public Task<BaseResult<PagedList<UserResponseDto>>> GetAll(ItemQueryParams queryParams, ObjectId senderId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<UserResponseDto>> GetByID(ObjectId userId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<ObjectId>> Update(ObjectId userId, UserRequestDto dto, ObjectId senderId)
        {
            throw new NotImplementedException();
        }
    }
}
