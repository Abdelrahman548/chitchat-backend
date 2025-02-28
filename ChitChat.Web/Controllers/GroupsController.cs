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
    public class GroupsController : ControllerBase
    {
        private readonly ServicesUnit services;

        public GroupsController(ServicesUnit services)
        {
            this.services = services;
        }

        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<GroupResponseDto>>>> GetGroups([FromQuery] ItemQueryParams queryParams)
        {
            BaseResult<PagedList<GroupResponseDto>> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<PagedList<GroupResponseDto>>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.GetAllGroups(queryParams, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("")]
        public async Task<ActionResult<BaseResult<GroupResponseDto>>> AddGroup(GroupRequestDto dto)
        {
            BaseResult<GroupResponseDto> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<GroupResponseDto>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.Add(dto, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{groupId}")]
        public async Task<ActionResult<BaseResult<GroupResponseDto>>> GetGroup(string groupId)
        {
            BaseResult<GroupResponseDto> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<GroupResponseDto>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.GetByID(groupId, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{groupId}")]
        public async Task<ActionResult<BaseResult<string>>> UpdateGroup(string groupId, GroupRequestDto dto)
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.Update(groupId, dto, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{groupId}/picture")]
        public async Task<ActionResult<BaseResult<string>>> UploadGroupPicture(string groupId, IFormFile image)
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.UploadPicture(groupId, image, authId);

            return StatusCode((int)result.StatusCode, result);
        }



        [HttpGet("{groupId}/admins")]
        public async Task<ActionResult<BaseResult<PagedList<UserResponseDto>>>> GetGroupAdmins(string groupId, [FromQuery] ItemQueryParams queryParams)
        {
            BaseResult<PagedList<UserResponseDto>> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<PagedList<UserResponseDto>>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.GetAllGroupAdmins(groupId, queryParams, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{groupId}/admins")]
        public async Task<ActionResult<BaseResult<string>>> AddGroupAdmin(string groupId, string memberId)
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.AddAdmin(groupId, memberId, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{groupId}/admins")]
        public async Task<ActionResult<BaseResult<string>>> RemoveGroupAdmin(string groupId, string memberId)
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.RemoveAdmin(groupId, memberId, authId);

            return StatusCode((int)result.StatusCode, result);
        }


        
        
        [HttpGet("{groupId}/members")]
        public async Task<ActionResult<BaseResult<PagedList<UserResponseDto>>>> GetGroupMembers(string groupId, [FromQuery] ItemQueryParams queryParams)
        {
            BaseResult<PagedList<UserResponseDto>> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<PagedList<UserResponseDto>>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.GetAllGroupMembers(groupId, queryParams, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{groupId}/members")]
        public async Task<ActionResult<BaseResult<string>>> AddGroupMember(string groupId, string userId)
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.AddMember(groupId, userId, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{groupId}/members")]
        public async Task<ActionResult<BaseResult<string>>> RemoveGroupMember(string groupId, string userId)
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.RemoveMember(groupId, userId, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        
        
        
        [HttpPost("{groupId}/chat/messages")]
        public async Task<ActionResult<BaseResult<MessageResponseDto>>> SendMessage(string groupId, MessageRequestDto dto)
        {
            BaseResult<MessageResponseDto> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<MessageResponseDto>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupMessageService.Add(groupId, dto, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("chat/messages")]
        public async Task<ActionResult<BaseResult<MessageResponseDto>>> GetMessage(string messageId)
        {
            BaseResult<MessageResponseDto> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<MessageResponseDto>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupMessageService.GetByID(messageId, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{groupId}/chat/messages")]
        public async Task<ActionResult<BaseResult<string>>> DeleteMessage(string groupId, string messageId)
        {
            BaseResult<string> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupMessageService.Delete(groupId, messageId, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("chat/messages")]
        public async Task<ActionResult<BaseResult<MessageResponseDto>>> UpdateMessage(string messageId, MessageRequestDto dto)
        {
            BaseResult<MessageResponseDto> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<MessageResponseDto>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupMessageService.Update(messageId, dto, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        
        
        [HttpGet("{groupId}/chat")]
        public async Task<ActionResult<BaseResult<PagedList<MessageResponseDto>>>> GetGroupChat(string groupId, [FromQuery] ItemQueryParams queryParams)
        {
            BaseResult<PagedList<MessageResponseDto>> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<PagedList<MessageResponseDto>>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupMessageService.GetAll(groupId, queryParams, authId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("User")]
        public async Task<ActionResult<BaseResult<PagedList<GroupResponseDto>>>> GetUserGroups([FromQuery] ItemQueryParams queryParams)
        {
            BaseResult<PagedList<GroupResponseDto>> result;
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                result = new BaseResult<PagedList<GroupResponseDto>>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.GroupService.GetAllUserGroups(queryParams, authId);

            return StatusCode((int)result.StatusCode, result);
        }


    }
}
