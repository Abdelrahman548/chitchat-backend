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
    public class FriendsController : ControllerBase
    {
        private readonly ServicesUnit services;

        public FriendsController(ServicesUnit services)
        {
            this.services = services;
        }

        [HttpPost("requests/send")]
        public async Task<ActionResult<BaseResult<FriendResponseDto>>> Send(FriendRequestDto dto)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            dto.SenderId = authId;
            var result = await services.FriendService.Add(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("requests/{friendRequestId}/accept")]
        public async Task<ActionResult<BaseResult<string>>> Accept(string friendRequestId)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.FriendService.Accept(friendRequestId, authId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("requests/{friendRequestId}/cancel")]
        public async Task<ActionResult<BaseResult<string>>> Cancel(string friendRequestId)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.FriendService.Cancel(friendRequestId, authId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("requests/{friendRequestId}")]
        public async Task<ActionResult<BaseResult<FriendResponseDto>>> Get(string friendRequestId)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.FriendService.GetByID(friendRequestId, authId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("requests/sent")]
        public async Task<ActionResult<BaseResult<string>>> GetUserSentFriendRequests([FromQuery]ItemQueryParams queryParams)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.FriendService.GetAllSent(queryParams, authId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("requests/received")]
        public async Task<ActionResult<BaseResult<string>>> GetUserReceivedFriendRequests([FromQuery] ItemQueryParams queryParams)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.FriendService.GetAllReceived(queryParams, authId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("all-friends")]
        public async Task<ActionResult<BaseResult<string>>> Friends([FromQuery] ItemQueryParams queryParams)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.FriendService.GetAllFriends(queryParams, authId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
