using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Service.Implementations
{
    public class CloudinaryService : ICloudService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }
        public async Task<bool> DeleteAllByTagAsync(string tag)
        {
            var deleteResult = await cloudinary.DeleteResourcesByTagAsync(tag);

            return (deleteResult.Deleted.Count > 0);
        }
        public async Task<BaseResult<string>> UploadImageAsync(IFormFile image, string tag)
        {
            using var stream = image.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, stream),
                Transformation = new Transformation().Quality("auto").FetchFormat("auto"),
                Tags = tag
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            var link = uploadResult?.SecureUrl?.ToString();

            if(link is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Message = "something went wrong, try again later", Data = link };

            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = "Image is uploaded successfully", Data = link };
        }

        public async Task<BaseResult<string>> UploadVideoAsync(IFormFile video, string tag)
        {
            using var stream = video.OpenReadStream();
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(video.FileName, stream),
                Tags = tag
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            var link = uploadResult?.SecureUrl?.ToString();

            if (link is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Message = "Something went wrong, try again later", Data = link };

            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = "Video is uploaded successfully", Data = link };
        }

        public async Task<bool> DeleteResourceByUrlAsync(string url)
        {
            
            var publicId = GetPublicIdFromUrl(url);

            
            var resourceType = url.Contains("/image/") ? ResourceType.Image : ResourceType.Video;

            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = resourceType
            };

            var deleteResult = await cloudinary.DestroyAsync(deleteParams);

            return (deleteResult.Result == "ok") ;
        }

        private string GetPublicIdFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.Segments;
            var publicId = segments.Last().Split('.').First();
            return publicId;
        }
    }
}
