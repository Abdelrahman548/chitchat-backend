using ChitChat.Service.DTOs.Request;
using ChitChat.Service.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChitChat.Web.Hubs
{
    [Authorize(Roles = "User")]
    public class ChatHub : Hub
    {
        private readonly ServicesUnit services;

        public ChatHub(ServicesUnit services)
        {
            this.services = services;
        }

        public async Task SendMessage(string receiverId, MessageRequestDto message)
        {
            string senderId = GetUserId();
            message.ReceiverId = receiverId;
            var result = await services.ChatMessageService.Add(message, senderId);
            if(result.IsSuccess)
                await Clients.User(receiverId).SendAsync("ReceiveChatMessage", senderId, message);
        }

        public async Task SendMessageToGroup(string groupId, MessageRequestDto message)
        {
            string senderId = GetUserId();
            var result = await services.GroupMessageService.Add(groupId, message, senderId);
            if (result.IsSuccess)
                await Clients.Group(groupId).SendAsync("ReceiveGroupMessage", senderId, message);
        }

        public async Task JoinGroup(string groupId)
        {
            string senderId = GetUserId();
            await Groups.AddToGroupAsync(senderId, groupId);
            await Clients.Group(groupId).SendAsync("UserJoinedGroup", senderId);
        }

        public async Task LeaveGroup(string groupId)
        {
            string senderId = GetUserId();
            await Groups.RemoveFromGroupAsync(senderId, groupId);
            await Clients.Group(groupId).SendAsync("UserLeftGroup", senderId);
        }

        public override async Task OnConnectedAsync()
        {
            string userId = GetUserId();
            await Clients.All.SendAsync("UserConnected", userId);
            await base.OnConnectedAsync();
        }

        private string GetUserId()
        {
            return Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new HubException("Unauthorized");
        }
    }
}
