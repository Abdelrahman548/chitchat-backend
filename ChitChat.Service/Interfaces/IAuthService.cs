using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using MongoDB.Bson;

namespace ChitChat.Service.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResult<LoginResponseDto>> Login(LoginRequestDto dto);
        Task<BaseResult<string>> Logout(string userId);
        Task<BaseResult<string>> Register(RegisterRequestDto dto);
        Task<BaseResult<string>> ForgetPassword(VerifyEmailRequestDto dto);
        Task<BaseResult<string>> ResetPassword(ResetPasswordRequestDto dto);
        Task<BaseResult<string>> VerifyEmail(VerifyEmailRequestDto dto);
        Task<BaseResult<LoginResponseDto>> RefreshToken(RefreshRequestDto refreshRequest);
    }
}
