using ChitChat.Service.Interfaces;

namespace ChitChat.Service.Implementations
{
    public class ServicesUnit(IAuthService authService, IEmailService emailService, IOTPService oTPService, ICloudService cloudService, IUserService userService, IGroupService groupService, IGroupMessageService groupMessageService, IChatMessageService chatMessageService, IFriendService friendService, ITokenService tokenService, IStatusService statusService)
    {
        public IAuthService AuthService { get; } = authService;
        public IEmailService EmailService { get; } = emailService;
        public IOTPService OTPService { get; } = oTPService;
        public ICloudService CloudService { get; } = cloudService;
        public IUserService UserService { get; } = userService;
        public IGroupService GroupService { get; } = groupService;
        public IGroupMessageService GroupMessageService { get; } = groupMessageService;
        public IChatMessageService ChatMessageService { get; } = chatMessageService;
        public IFriendService FriendService { get; } = friendService;
        public ITokenService TokenService { get; } = tokenService;
        public IStatusService StatusService { get; } = statusService;
    }
}
