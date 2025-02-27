using AutoMapper;
using ChitChat.Repository.Helpers;
using ChitChat.Repository.Interfaces;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Data.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;
using ChitChat.Data.Entities;

namespace ChitChat.Service.Implementations
{
    public class StatusService : IStatusService
    {
        private readonly IUnitOfWork repoUnit;
        private readonly IMapper mapper;
        private readonly ICloudService cloudService;

        public StatusService(IUnitOfWork repoUnit, IMapper mapper, ICloudService cloudService)
        {
            this.repoUnit = repoUnit;
            this.mapper = mapper;
            this.cloudService = cloudService;
        }
        public async Task<BaseResult<StatusResponseDto>> Add(ObjectId userId, StatusRequestDto dto)
        {
            var user = await repoUnit.Users.GetByIdAsync(userId);
            if (user is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Not Found"] };
            
            Status status = new Status() { Id = ObjectId.GenerateNewId(), ContentType = dto.ContentType, ExpirationTime = DateTime.UtcNow.AddDays(1), Text = dto.Text, UserId = userId };
            user.StatusIds.Add(status.Id);
            BaseResult<string>? result = null;
            switch (dto.ContentType)
            {
                case ContentType.Image:
                    result = await cloudService.UploadImageAsync(dto.Payload, $"Status[{status.Id}]For[{userId}]");
                    break;
                case ContentType.Video:
                    result = await cloudService.UploadVideoAsync(dto.Payload, $"Status[{status.Id}]For[{userId}]");
                    break;
            }
            if(result is not null)
            {
                var link = result.Data;
                if (!result.IsSuccess || link is null || result.StatusCode != MyStatusCode.OK)
                    return new() { IsSuccess = false, StatusCode = result.StatusCode, Errors = result.Errors };
                status.PayloadUrl = link;
            }

            await repoUnit.Users.UpdateAsync(userId, user);
            await repoUnit.Statuses.AddAsync(status);
            var responseDto = mapper.Map<StatusResponseDto>(status);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Data = responseDto , Message = Messages.ADD_SUCCESS};
        }

        public async Task<BaseResult<string>> Delete(ObjectId statusId, ObjectId senderId)
        {
            var status = await repoUnit.Statuses.GetByIdAsync(statusId);
            if (status is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Not Found"] };

            if (status.UserId != senderId)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Not Found"] };

            var user = await repoUnit.Users.GetByIdAsync(senderId);
            if (user is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Not Found"] };

            if(!user.StatusIds.Contains(statusId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Forbidden"] };

            if (status.ContentType != ContentType.Text)
            {
                var isDeleted = await cloudService.DeleteResourceByUrlAsync(status.PayloadUrl);
                if (!isDeleted)
                    return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Something went wrong, try again later"] };
            }
            user.StatusIds.Remove(statusId);
            var _ = repoUnit.Statuses.DeleteAsync(statusId);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.DELETE_SUCCESS };
        }

        public async Task<BaseResult<PagedList<StatusResponseDto>>> GetAll(ObjectId userId, ItemQueryParams queryParams, ObjectId senderId)
        {
            var user = await repoUnit.Users.GetByIdAsync(senderId);
            if(!user.FriendsIds.Contains(userId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Not Found"] };

            var pageList = await repoUnit.Statuses.GetAllAsync(queryParams, s => s.UserId == userId);
            var responsePageList = new PagedList<StatusResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<StatusResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
            );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<StatusResponseDto>> GetByID(ObjectId statusId, ObjectId senderId)
        {
            var status = await repoUnit.Statuses.GetByIdAsync(statusId);
            if (status is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Not Found"] };

            var sender = await repoUnit.Users.GetByIdAsync(senderId);
            if(!sender.FriendsIds.Contains(status.UserId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Not Found"] };

            var responseDto = mapper.Map<StatusResponseDto>(status);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.GET_SUCCESS, Data = responseDto };
        }
    }
}
