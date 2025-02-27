﻿using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IChatMessageService
    {   
        Task<BaseResult<PagedList<MessageResponseDto>>> GetAll(ObjectId chatId, ItemQueryParams queryParams, ObjectId memberId);
        Task<BaseResult<MessageResponseDto>> GetByID(ObjectId messageId, ObjectId memberId);
        Task<BaseResult<MessageResponseDto>> Add(MessageRequestDto dto, ObjectId senderId);
        Task<BaseResult<MessageResponseDto>> Update(ObjectId messageId, MessageRequestDto dto, ObjectId senderId);
        Task<BaseResult<string>> Delete(ObjectId messageId, ObjectId senderId);
    }
}
