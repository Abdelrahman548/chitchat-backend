using ChitChat.Service.Helpers;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Service.Interfaces
{
    public interface ICloudService
    {
        Task<bool> DeleteAllByTagAsync(string tag);
        Task<bool> DeleteResourceByUrlAsync(string url);
        Task<BaseResult<string>> UploadImageAsync(IFormFile image, string userId);
        Task<BaseResult<string>> UploadVideoAsync(IFormFile video, string userId);
    }
}
