using ChitChat.Service.Helpers;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Service.Interfaces
{
    public interface ICloudService
    {
        Task<BaseResult<string>> UploadImageAsync(IFormFile image);
        Task<BaseResult<string>> UploadVideoAsync(IFormFile video);
    }
}
