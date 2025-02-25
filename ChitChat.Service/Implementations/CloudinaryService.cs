using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Service.Implementations
{
    public class CloudinaryService : ICloudService
    {
        public Task<BaseResult<string>> UploadImageAsync(IFormFile image)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<string>> UploadVideoAsync(IFormFile video)
        {
            throw new NotImplementedException();
        }
    }
}
