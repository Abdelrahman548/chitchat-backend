using ChitChat.Data.Entities;
using ChitChat.Repository.Helpers;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace ChitChat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly ServicesUnit services;

        public ChatsController(ServicesUnit services)
        {
            this.services = services;
        }

        [Authorize(Roles = "User")]
        [HttpPost("messages")]
        public async Task<ActionResult<BaseResult<MessageResponseDto>>> SendMessage(MessageRequestDto dto)
        {
            BaseResult<MessageResponseDto> result;
            var IdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (IdStr is null)
            {
                result = new BaseResult<MessageResponseDto>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.ChatMessageService.Add(dto, ObjectId.Parse(IdStr));
            
            return StatusCode((int)result.StatusCode, result);
        }
        
        [Authorize(Roles = "User")]
        [HttpGet("messages")]
        public async Task<ActionResult<BaseResult<MessageResponseDto>>> GetMessage(string messageId)
        {
            BaseResult<MessageResponseDto> result;
            var IdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (IdStr is null)
            {
                result = new BaseResult<MessageResponseDto>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.ChatMessageService.GetByID(ObjectId.Parse(messageId), ObjectId.Parse(IdStr));

            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("messages")]
        public async Task<ActionResult<BaseResult<string>>> DeleteMessage(ObjectId messageId)
        {
            BaseResult<string> result;
            var IdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (IdStr is null)
            {
                result = new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.ChatMessageService.Delete(messageId, ObjectId.Parse(IdStr));

            return StatusCode((int)result.StatusCode, result);
        }
        
        [Authorize(Roles = "User")]
        [HttpPut("messages")]
        public async Task<ActionResult<BaseResult<MessageResponseDto>>> UpdateMessage(ObjectId messageId, [FromBody]MessageRequestDto dto)
        {
            BaseResult<MessageResponseDto> result;
            var IdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (IdStr is null)
            {
                result = new BaseResult<MessageResponseDto>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.ChatMessageService.Update(messageId, dto, ObjectId.Parse(IdStr));

            return StatusCode((int)result.StatusCode, result);
        }

        
        [Authorize(Roles = "User")]
        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<MessageResponseDto>>>> GetChat(ObjectId chatId, ItemQueryParams queryParams)
        {
            BaseResult<PagedList<MessageResponseDto>> result;
            var IdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (IdStr is null)
            {
                result = new BaseResult<PagedList<MessageResponseDto>>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized };
            }
            else
                result = await services.ChatMessageService.GetAll(chatId,queryParams, ObjectId.Parse(IdStr));

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
