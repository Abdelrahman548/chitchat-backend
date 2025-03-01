using AutoMapper;
using ChitChat.Repository.Helpers;
using ChitChat.Repository.Interfaces;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork repoUnit;
        private readonly IMapper mapper;
        private readonly ICloudService cloudService;

        public UserService(IUnitOfWork repoUnit, IMapper mapper, ICloudService cloudService)
        {
            this.repoUnit = repoUnit;
            this.mapper = mapper;
            this.cloudService = cloudService;
        }
        public async Task<BaseResult<PagedList<UserResponseDto>>> GetAll(ItemQueryParams queryParams, string senderId)
        {
            var pageList = await repoUnit.Users.GetAllAsync(queryParams);
            var responsePageList = new PagedList<UserResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<UserResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
            );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<UserResponseDto>> GetByID(string userId)
        {
            var user = await repoUnit.Users.GetByIdAsync(userId);
            if (user is null) return new BaseResult<UserResponseDto>() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            var responseDto = mapper.Map<UserResponseDto>(user);
            return new BaseResult<UserResponseDto>() { IsSuccess = true, Data = responseDto, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<string>> Update(string userId, UserRequestDto dto, string senderId)
        {
            var user = await repoUnit.Users.GetByIdAsync(userId);
            if (user is null) return new() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            mapper.Map(dto, user);
            await repoUnit.Users.UpdateAsync(user.Id,user);
            return new() { IsSuccess = true, Message = Messages.UPDATE_SUCCESS, Data = user.Id, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<string>> UpdateLastseen(DateTime lastseen, string senderId)
        {
            var user = await repoUnit.Users.GetByIdAsync(senderId);
            if (user is null) return new() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            user.LastSeen = lastseen;
            await repoUnit.Users.UpdateAsync(user.Id, user);
            return new() { IsSuccess = true, Message = Messages.UPDATE_SUCCESS, Data = user.Id, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<string>> UploadPicture(IFormFile image, string senderId)
        {
            var user = await repoUnit.Users.GetByIdAsync(senderId);
            if (user is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["User is not Found"] };

            var result = await cloudService.UploadImageAsync(image, $"ProfilePicture[{senderId}]");
            var link = result.Data;
            if (!result.IsSuccess || link is null || result.StatusCode != MyStatusCode.OK)
                return result;

            user.PictureUrl = link;


            await repoUnit.Users.UpdateAsync(senderId, user);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.UPDATE_SUCCESS };
        }
    }
}
