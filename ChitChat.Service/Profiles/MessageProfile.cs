using AutoMapper;
using ChitChat.Data.Entities;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;

namespace ChitChat.Service.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageRequestDto, Message>();
            CreateMap<Message, MessageResponseDto>();
        }
    }
}
