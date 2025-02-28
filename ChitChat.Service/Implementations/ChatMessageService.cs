using AutoMapper;
using ChitChat.Data.Entities;
using ChitChat.Repository.Helpers;
using ChitChat.Repository.Interfaces;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Data.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ChitChat.Service.Implementations
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IUnitOfWork repoUnit;
        private readonly IMapper mapper;
        private readonly ICloudService cloudService;

        public ChatMessageService(IUnitOfWork repoUnit, IMapper mapper, ICloudService cloudService)
        {
            this.repoUnit = repoUnit;
            this.mapper = mapper;
            this.cloudService = cloudService;
        }
        public async Task<BaseResult<MessageResponseDto>> Add(MessageRequestDto dto, string senderId)
        {
            var sender = await repoUnit.Users.GetByIdAsync(senderId);
            if(sender is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["User is not Found"] };
            if(!sender.FriendsIds.Contains(dto.ReceiverId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["User is not Found"] };
            var chats = await repoUnit.Chats.FindAsync(c => c.FirstUserId == senderId && c.SecondUserId == dto.ReceiverId);
            Chat chat;
            if (chats.Count == 0)
            {
                chat = new Chat() { Id = ObjectId.GenerateNewId().ToString(), FirstUserId = senderId, SecondUserId = dto.ReceiverId };
                await repoUnit.Chats.AddAsync(chat);
                
                var receiver = await repoUnit.Users.GetByIdAsync(dto.ReceiverId);
                
                sender.ChatsIds.Add(chat.Id);
                receiver.ChatsIds.Add(chat.Id);
            }
            else
                chat = chats[0];
            
            var message = mapper.Map<Message>(dto);
            message.Id = ObjectId.GenerateNewId().ToString();
            message.ChatId = chat.Id;
            message.ChatType = ChatType.Private;
            message.SenderId = senderId;

            BaseResult<string> result = null;
            switch(dto.ContentType)
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

        public async Task<BaseResult<string>> Delete(string messageId, string senderId)
        {
            var message = await repoUnit.Messages.GetByIdAsync(messageId);

            if (message is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Message is not Found"] };
            if (message.SenderId != senderId && message.ReceiverId != senderId)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Forbidden"] };

            if (message.ContentType != ContentType.Text)
                await cloudService.DeleteResourceByUrlAsync(message.PayloadUrl);

            await repoUnit.Messages.DeleteAsync(messageId);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.DELETE_SUCCESS};
        }

        public async Task<BaseResult<PagedList<MessageResponseDto>>> GetAll(string chatId, ItemQueryParams queryParams, string memberId)
        {
            var chat = await repoUnit.Chats.GetByIdAsync(chatId);
            if (chat.FirstUserId != memberId && chat.SecondUserId != memberId)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Not Found"] };

            var pageList = await repoUnit.Messages.GetAllAsync(queryParams, m => m.ChatId == chatId);
            var responsePageList = new PagedList<MessageResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<MessageResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
            );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<MessageResponseDto>> GetByID(string messageId, string memberId)
        {
            var message = await repoUnit.Messages.GetByIdAsync(messageId);
            if(message is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Message is not Found"] };
            if(message.SenderId != memberId && message.ReceiverId != memberId)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Forbidden"] };

            var responseDto = mapper.Map<MessageResponseDto>(message);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.GET_SUCCESS, Data = responseDto };
        }

        public async Task<BaseResult<MessageResponseDto>> Update(string messageId, MessageRequestDto dto, string senderId)
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
