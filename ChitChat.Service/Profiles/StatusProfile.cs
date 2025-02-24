using AutoMapper;
using ChitChat.Data.Entities;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;

namespace ChitChat.Service.Profiles
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<StatusRequestDto, Status>();
            CreateMap<Status, StatusResponseDto>();
        }
    }
}
