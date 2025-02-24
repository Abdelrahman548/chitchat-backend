using AutoMapper;
using ChitChat.Data.Entities;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;

namespace ChitChat.Service.Profiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupRequestDto, Group>();
            CreateMap<Group, GroupResponseDto>();
        }
    }
}
