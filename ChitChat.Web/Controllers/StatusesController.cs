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
    public class StatusesController : ControllerBase
    {
        private readonly ServicesUnit services;

        public StatusesController(ServicesUnit services)
        {
            this.services = services;
        }

        [HttpPost("")]
        public async Task<ActionResult<BaseResult<StatusResponseDto>>> AddStatus(StatusRequestDto dto)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }

            var result = await services.StatusService.Add(authId, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{statusId}")]
        public async Task<ActionResult<BaseResult<LoginResponseDto>>> DeleteStatus(string statusId)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }

            var result = await services.StatusService.Delete(statusId, authId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{statusId}")]
        public async Task<ActionResult<BaseResult<string>>> GetById(string statusId)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.StatusService.GetByID(statusId, authId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<BaseResult<string>>> GetAllFriendStatuses(string userId,[FromQuery] ItemQueryParams queryParams)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.StatusService.GetAll(userId, queryParams, authId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("user/")]
        public async Task<ActionResult<BaseResult<string>>> GetAllUserStatuses([FromQuery] ItemQueryParams queryParams)
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.StatusService.GetAll(authId, queryParams, authId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
