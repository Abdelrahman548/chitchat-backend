using ChitChat.Service.Helpers;

namespace ChitChat.Service.DTOs.Response
{
    public class LoginResponseDto
    {
        public Token? Token { get; set; }
        public UserResponseDto? User { get; set; }
    }
}
