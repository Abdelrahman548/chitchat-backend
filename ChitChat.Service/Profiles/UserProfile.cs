using AutoMapper;
using ChitChat.Data.Entities;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;

namespace ChitChat.Service.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequestDto, User>();
            CreateMap<User, UserResponseDto>();
        }
    }
}
