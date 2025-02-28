using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Implementations;
using ChitChat.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChitChat.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ServicesUnit services;

        public AuthController(ServicesUnit services)
        {
            this.services = services;
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResult<LoginResponseDto>>> Login(LoginRequestDto dto)
        {
            var result = await services.AuthService.Login(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<BaseResult<LoginResponseDto>>> Refresh(RefreshRequestDto dto)
        {
            var result = await services.AuthService.RefreshToken(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "User")]
        [HttpPost("logout")]
        public async Task<ActionResult<BaseResult<string>>> Logout()
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.AuthService.Logout(new(authId));
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("")]
        public async Task<ActionResult<BaseResult<UserResponseDto>>> Current()
        {
            var authId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authId is null)
            {
                return Unauthorized(
                        new BaseResult<string>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await services.UserService.GetByID(new(authId));
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<BaseResult<string>>> Register(RegisterRequestDto dto)
        {
            var result = await services.AuthService.Register(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpPost("email/verify")]
        public async Task<ActionResult<BaseResult<string>>> VerifyEmail(VerifyEmailRequestDto verifyDto)
        {
            var result = await services.AuthService.VerifyEmail(verifyDto);
            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpPost("password/forgot")]
        public async Task<ActionResult<BaseResult<string>>> ForgotPassword(VerifyEmailRequestDto verifyDto)
        {
            var result = await services.AuthService.ForgetPassword(verifyDto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("password/reset")]
        public async Task<ActionResult<BaseResult<string>>> ResetPassword(ResetPasswordRequestDto dto)
        {
            var result = await services.AuthService.ResetPassword(dto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
