using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChitChat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class UsersController : ControllerBase
    {
        private readonly ServicesUnit services;

        public UsersController(ServicesUnit services)
        {
            this.services = services;
        }

        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<UserResponseDto>>>> GetUsers([FromQuery] ItemQueryParams queryParams)
        {
            BaseResult<PagedList<UserResponseDto>> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<PagedList<UserResponseDto>>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.UserService.GetAll(queryParams, authId);

            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet("{userId}")]
        public async Task<ActionResult<BaseResult<UserResponseDto>>> GetUser(string userId)
        {
            var result = await services.UserService.GetByID(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("current")]
        public async Task<ActionResult<BaseResult<UserResponseDto>>> GetCurrent()
        {
            BaseResult<UserResponseDto> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<UserResponseDto>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.UserService.GetByID(authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("")]
        public async Task<ActionResult<BaseResult<string>>> UpdateUser(UserRequestDto dto)
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.UserService.Update(authId, dto, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("lastseen")]
        public async Task<ActionResult<BaseResult<string>>> UpdateUserLastSeen()
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.UserService.UpdateLastseen(DateTime.Now, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("picture")]
        public async Task<ActionResult<BaseResult<string>>> UploadUserPicture(IFormFile image)
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.UserService.UploadPicture(image, authId);

            return StatusCode((int)result.StatusCode, result);
        }
    
    }
}
