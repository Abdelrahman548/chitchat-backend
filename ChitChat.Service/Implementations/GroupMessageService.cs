using AutoMapper;
using ChitChat.Data.Entities;
using ChitChat.Data.Helpers;
using ChitChat.Repository.Helpers;
using ChitChat.Repository.Interfaces;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;

namespace ChitChat.Service.Implementations
{
    public class GroupMessageService : IGroupMessageService
    {
        private readonly IUnitOfWork repoUnit;
        private readonly IMapper mapper;
        private readonly ICloudService cloudService;

        public GroupMessageService(IUnitOfWork repoUnit, IMapper mapper, ICloudService cloudService)
        {
            this.repoUnit = repoUnit;
            this.mapper = mapper;
            this.cloudService = cloudService;
        }
        public async Task<BaseResult<MessageResponseDto>> Add(ObjectId groupId, MessageRequestDto dto, ObjectId senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };
            
            if(group.MembersIds.Contains(senderId))
            {
                var canSend = (group.Permissions & GroupPermissions.SendMessages) != 0;
                if(!canSend)
                    return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Only Admins can send messages"] };
            }
            
            var message = mapper.Map<Message>(dto);
            message.Id = ObjectId.GenerateNewId();
            message.ChatId = groupId;
            message.ChatType = ChatType.Group;
            message.SenderId = senderId;
            message.ReceiverId = ObjectId.Empty;

            BaseResult<string> result = null;
            switch (dto.ContentType)
            {
                case ContentType.Image:
                    result = await cloudService.UploadImageAsync(dto.Payload, $"Message[{message.Id}]For[{senderId}][{dto.ReceiverId}]");
                    break;
                case ContentType.Video:
                    result = await cloudService.UploadVideoAsync(dto.Payload, $"Message[ {message.Id} ]For[ {senderId} ][ {dto.ReceiverId}]");
                    break;
            }
            if (result is not null)
            {
                var link = result.Data;
                if (!result.IsSuccess || link is null || result.StatusCode != MyStatusCode.OK)
                    return new() { IsSuccess = false, StatusCode = result.StatusCode, Errors = result.Errors };
                message.PayloadUrl = link;
            }
            var responseDto = mapper.Map<MessageResponseDto>(message);
            await repoUnit.Messages.AddAsync(message);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.ADD_SUCCESS, Data = responseDto };
        }

        public async Task<BaseResult<string>> Delete(ObjectId groupId, ObjectId messageId, ObjectId senderId)
        {
            var message = await repoUnit.Messages.GetByIdAsync(messageId);

            if (message is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Message is not Found"] };
            if (message.SenderId != senderId)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Forbidden"] };

            if (message.ContentType != ContentType.Text)
                await cloudService.DeleteResourceByUrlAsync(message.PayloadUrl);
            
            await repoUnit.Messages.DeleteAsync(messageId);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.DELETE_SUCCESS };
        }

        public async Task<BaseResult<PagedList<MessageResponseDto>>> GetAll(ObjectId groupId, ItemQueryParams queryParams, ObjectId memberId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };

            if (!group.MembersIds.Contains(memberId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Not Found"] };

            var pageList = await repoUnit.Messages.GetAllAsync(queryParams, m => m.ChatId == groupId);
            var responsePageList = new PagedList<MessageResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<MessageResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
            );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<MessageResponseDto>> GetByID(ObjectId messageId, ObjectId memberId)
        {
            var message = await repoUnit.Messages.GetByIdAsync(messageId);
            if (message is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Message is not Found"] };
            
            var group = await repoUnit.Groups.GetByIdAsync(message.ChatId);
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };
            
            if (!group.MembersIds.Contains(memberId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Not Found"] };
            
            
            var responseDto = mapper.Map<MessageResponseDto>(message);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.GET_SUCCESS, Data = responseDto };
        }

        public async Task<BaseResult<MessageResponseDto>> Update(ObjectId messageId, MessageRequestDto dto, ObjectId senderId)
        {
            var message = await repoUnit.Messages.GetByIdAsync(messageId);

            if (message is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Message is not Found"] };
            if (message.SenderId != senderId)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Forbidden"] };

            message.Text = dto.Text;
            var responseDto = mapper.Map<MessageResponseDto>(message);
            await repoUnit.Messages.UpdateAsync(message.Id, message);

            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.UPDATE_SUCCESS, Data = responseDto };
        }
    }
}
