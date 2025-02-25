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
            CreateMap<User, UserResponseDto>().ForMember(dest => dest.LastSeen, opt => opt.MapFrom((src, dest) =>
                src.LastSeenVisability ? src.LastSeen : null
            ));
        }
    }
}
