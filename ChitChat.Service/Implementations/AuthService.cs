using ChitChat.Data.Entities;
using ChitChat.Repository.Interfaces;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;

namespace ChitChat.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork repoUnit;
        private readonly ITokenService tokenService;
        private readonly IOTPService otpService;
        private readonly IEmailService emailService;

        public AuthService(IUnitOfWork repoUnit, ITokenService tokenService, IOTPService otpService, IEmailService emailService)
        {
            this.repoUnit = repoUnit;
            this.tokenService = tokenService;
            this.otpService = otpService;
            this.emailService = emailService;
        }
        public async Task<BaseResult<string>> ForgetPassword(VerifyEmailRequestDto dto)
        {
            var user = await repoUnit.Users.FindAsync(E => E.Email == dto.Email);
            if (user is null || user.Count == 0)
                return new() { IsSuccess = false, Errors = ["Invalid Email"], StatusCode = MyStatusCode.BadRequest };
            int timeInMinutes = 5;
            string subject = "Your OTP for Email Verfication";
            string otp = otpService.GenerateOTP();
            try
            {
                string body = otpService.GetBodyTemplate(otp, timeInMinutes, "ChitChat");
                await emailService.SendEmailWithHtml($"{user[0].Name}", dto.Email, subject, body);
            }
            catch
            {
                // Log The Exception and Stack Trace
                return new() { IsSuccess = false, Errors = ["Something went wrong, please try again later"], StatusCode = MyStatusCode.BadRequest };
            }
            var otpVerify = await repoUnit.EmailOTPVerfications.FindAsync(E => E.Email == dto.Email);
            var expirationTime = DateTime.UtcNow.AddMinutes(timeInMinutes);
            var hashedOtp = HashingManager.HashPassword(otp);
            if (otpVerify is null || otpVerify.Count == 0)
            {
                var otpRecord = new EmailOTPVerfication() { Id = ObjectId.GenerateNewId(), Email = user[0].Email, HashedOTP = hashedOtp, ExpirationTime = expirationTime };
                await repoUnit.EmailOTPVerfications.AddAsync(otpRecord);
            }
            else
            {
                otpVerify[0].HashedOTP = hashedOtp;
                otpVerify[0].ExpirationTime = expirationTime;
                await repoUnit.EmailOTPVerfications.UpdateAsync(otpVerify[0].Id, otpVerify[0]);
            }
            return new() { IsSuccess = true, Message = "OTP is sent, Please check your email box", StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            var banned = await repoUnit.BannedEmails.FindAsync(e => e.Email == dto.Email);
            if (banned is not null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Invalid Email or Password"] };

            var user = await repoUnit.Users.FindAsync(e => e.Email == dto.Email);
            if (user is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Invalid Email or Password"] };
            if (!HashingManager.VerifyPassword(dto.Password, user[0].Password))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Invalid Email or Password"] };


            var accessToken = tokenService.GenerateAccessToken(user[0]);
            var refreshToken = tokenService.GenerateRefreshToken();
            
            RefreshToken refreshTokenRecord;
            if (ObjectId.Empty != user[0].RefreshTokenId)
            {
                refreshTokenRecord = await repoUnit.RefreshTokens.GetByIdAsync(user[0].RefreshTokenId);
                refreshTokenRecord.Token = refreshToken;
                refreshTokenRecord.IsUsed = false;
            }
            else
            {
                refreshTokenRecord = new()
                {
                    Id = ObjectId.GenerateNewId(),
                    Token = refreshToken,
                    IsUsed = false, 
                    IsRevoked = false,
                    ExpirationTime = DateTime.UtcNow.AddDays(7), 
                    UserId = user[0].Id
                };
                
                user[0].RefreshTokenId = refreshTokenRecord.Id;
                await repoUnit.Users.UpdateAsync(user[0].Id, user[0]);
                await repoUnit.RefreshTokens.AddAsync(refreshTokenRecord);
            }
            var loginResponse = new LoginResponseDto() { Token = new() { AccessToken = accessToken, RefreshToken = refreshToken } };
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Data = loginResponse, Message = "Logged in successfully"};
        }

        public async Task<BaseResult<string>> Logout(ObjectId userId)
        {
            var refreshTokenRecord = await repoUnit.RefreshTokens.FindAsync(e => e.UserId == userId);
            if(refreshTokenRecord is null || refreshTokenRecord.Count == 0)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Something went wrong"] };

            refreshTokenRecord[0].IsRevoked = true;
            await repoUnit.RefreshTokens.UpdateAsync(refreshTokenRecord[0].Id, refreshTokenRecord[0]);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = "Logged out successfully" };
        }

        public async Task<BaseResult<LoginResponseDto>> RefreshToken(RefreshRequestDto refreshRequest)
        {
            var user = await repoUnit.Users.GetByIdAsync(refreshRequest.UserId);
            if (user is null)
            {
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };
            }

            var refreshTokenRecord = await repoUnit.RefreshTokens.FindAsync(R => R.UserId == refreshRequest.UserId);
            if (refreshTokenRecord == null || refreshTokenRecord[0].Token != refreshRequest.RefreshToken || refreshTokenRecord[0].ExpirationTime < DateTime.UtcNow || refreshTokenRecord[0].IsRevoked)
            {
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };
            }

            var newAccessToken = tokenService.GenerateAccessToken(user);

            var loginResponse = new LoginResponseDto() { Token = new() { AccessToken = newAccessToken, RefreshToken = refreshTokenRecord[0].Token }};
            return new() { IsSuccess = true, Data = loginResponse, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<string>> Register(RegisterRequestDto dto)
        {
            var users = await repoUnit.Users.FindAsync(E => E.Email == dto.Email);
            if (users is not null || users?.Count != 0)
                return new() { IsSuccess = false, Errors = ["Repeated email"], StatusCode = MyStatusCode.BadRequest };

            var otpVerify = await repoUnit.EmailOTPVerfications.FindAsync(E => E.Email == dto.Email);
            if (otpVerify is null || otpVerify.Count != 0)
                return new() { IsSuccess = false, Errors = ["The Email is not verfied"], StatusCode = MyStatusCode.BadRequest };

            if (!HashingManager.VerifyPassword(dto.OTPEmailVerifyCode, otpVerify[0].HashedOTP))
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };

            if (otpVerify[0].ExpirationTime < DateTime.UtcNow)
                return new() { IsSuccess = false, Errors = ["Invalid Credintials"], StatusCode = MyStatusCode.BadRequest };

            var hashedPass = HashingManager.HashPassword(dto.Password);
            var userRecord = new User() { Id = ObjectId.GenerateNewId(), Email = dto.Email, Password = hashedPass };

            
            await repoUnit.Users.AddAsync(userRecord);
            return new() { IsSuccess = true, Message = "Registered Successfully", StatusCode = MyStatusCode.Created };
        }

        public async Task<BaseResult<string>> ResetPassword(ResetPasswordRequestDto dto)
        {
            var otpVerify = await repoUnit.EmailOTPVerfications.FindAsync(E => E.Email == dto.Email);
            if (otpVerify is null || otpVerify.Count == 0)
                return new() { IsSuccess = false, Errors = ["Invalid Creadentials"], StatusCode = MyStatusCode.BadRequest };
            if (otpVerify[0].ExpirationTime < DateTime.UtcNow)
                return new() { IsSuccess = false, Errors = ["Invalid Creadentials"], StatusCode = MyStatusCode.BadRequest };
            if (!HashingManager.VerifyPassword(dto.OTPCode, otpVerify[0].HashedOTP))
                return new() { IsSuccess = false, Errors = ["Invalid Creadentials"], StatusCode = MyStatusCode.BadRequest };

            var users = await repoUnit.Users.FindAsync(E => E.Email == dto.Email);
            if (users is null || users.Count == 0)
                return new() { IsSuccess = false, Errors = ["Invalid Email"], StatusCode = MyStatusCode.BadRequest };

            users[0].Password = HashingManager.HashPassword(dto.NewPassword);
            await repoUnit.Users.UpdateAsync(users[0].Id, users[0]);
            return new() { IsSuccess = true, Message = "Password is reset Successfully", StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<string>> VerifyEmail(VerifyEmailRequestDto dto)
        {
            int timeInMinutes = 5;
            string subject = "Your OTP for Email Verfication";
            string otp = otpService.GenerateOTP();
            try
            {
                string body = otpService.GetBodyTemplate(otp, timeInMinutes, "TaskManager App");
                await emailService.SendEmailWithHtml($"Guest", dto.Email, subject, body);
            }
            catch
            {
                return new() { IsSuccess = false, Errors = ["Something went wrong, please try again later"], StatusCode = MyStatusCode.BadRequest };
            }
            var otpVerify = await repoUnit.EmailOTPVerfications.FindAsync(E => E.Email == dto.Email);
            var expirationTime = DateTime.UtcNow.AddMinutes(timeInMinutes);
            var hashedOtp = HashingManager.HashPassword(otp);
            if (otpVerify is null || otpVerify.Count == 0)
            {
                var otpRecord = new EmailOTPVerfication() { Id = ObjectId.GenerateNewId(), Email = dto.Email, HashedOTP = hashedOtp, ExpirationTime = expirationTime };
                await repoUnit.EmailOTPVerfications.AddAsync(otpRecord);
            }
            else
            {
                otpVerify[0].HashedOTP = hashedOtp;
                otpVerify[0].ExpirationTime = expirationTime;
                await repoUnit.EmailOTPVerfications.UpdateAsync(otpVerify[0].Id, otpVerify[0]);
            }
            return new() { IsSuccess = true, Message = "OTP is sent, Please check your email box", StatusCode = MyStatusCode.OK };
        }
    }
}
