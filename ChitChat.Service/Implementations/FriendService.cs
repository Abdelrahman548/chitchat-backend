using AutoMapper;
using ChitChat.Data.Entities;
using ChitChat.Repository.Helpers;
using ChitChat.Repository.Interfaces;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;

namespace ChitChat.Service.Implementations
{
    public class FriendService : IFriendService
    {
        private readonly IUnitOfWork repoUnit;
        private readonly IMapper mapper;

        public FriendService(IUnitOfWork repoUnit, IMapper mapper)
        {
            this.repoUnit = repoUnit;
            this.mapper = mapper;
        }

        public async Task<BaseResult<string>> Accept(string friendRequestId, string senderId)
        {
            var friendRequest = await repoUnit.FriendRequests.GetByIdAsync(friendRequestId);
            if (friendRequest is null)
                return new() { IsSuccess = false, Errors = ["Invalid Friend Request ID"], StatusCode = MyStatusCode.BadRequest };
            if (friendRequest.ReceiverId != senderId)
                return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden };
            
            var users = await repoUnit.Users.FindAsync(u => u.Id == friendRequest.ReceiverId || u.Id == friendRequest.SenderId);
            users[0].FriendsIds.Add(users[1].Id);
            users[1].FriendsIds.Add(users[0].Id);
            
            foreach (var user in users)
            {
                user.FriendRequestsIds.Remove(friendRequestId);
                await repoUnit.Users.UpdateAsync(user.Id, user);
            }
            await repoUnit.FriendRequests.DeleteAsync(friendRequestId);
            return new() { IsSuccess = true, Message = "Friend Request is Canceled Successfully", StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<string>> Add(FriendRequestDto dto)
        {
            var users = await repoUnit.Users.FindAsync(u => u.Id == dto.ReceiverId || u.Id == dto.SenderId);
            if(users.Count != 2)
                return new() { IsSuccess = false, Errors = ["Invalid IDs"], StatusCode = MyStatusCode.BadRequest };

            if(users[0].BlockedUsersIds.Contains(users[1].Id)||
                users[1].BlockedUsersIds.Contains(users[0].Id)
            )
                return new() { IsSuccess = false, Errors = ["User is not Found"], StatusCode = MyStatusCode.BadRequest };

            var friendReq = new FriendRequest() { Id = ObjectId.GenerateNewId().ToString(), SenderId = dto.SenderId, ReceiverId = dto.ReceiverId };
            await repoUnit.FriendRequests.AddAsync(friendReq);
            foreach (var user in users)
            {
                user.FriendRequestsIds.Add(friendReq.Id);
                await repoUnit.Users.UpdateAsync(user.Id, user);
            }

            return new() { IsSuccess = true, Message = "Friend Request is Sent Successfully", StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<string>> Cancel(string friendRequestId, string senderId)
        {
            var friendRequest = await repoUnit.FriendRequests.GetByIdAsync(friendRequestId);
            if (friendRequest is null)
                return new() { IsSuccess = false, Errors = ["Invalid Friend Request ID"], StatusCode = MyStatusCode.BadRequest };
            if (friendRequest.SenderId != senderId && friendRequest.ReceiverId != senderId)
                return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden };

            var users = await repoUnit.Users.FindAsync(u => u.Id == friendRequest.ReceiverId || u.Id == friendRequest.SenderId);
            foreach (var user in users)
            {
                user.FriendRequestsIds.Remove(friendRequestId);
                await repoUnit.Users.UpdateAsync(user.Id, user);
            }
            await repoUnit.FriendRequests.DeleteAsync(friendRequestId);
            return new() { IsSuccess = true, Message = "Friend Request is Canceled Successfully", StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<PagedList<FriendResponseDto>>> GetAll(ItemQueryParams queryParams, string senderId)
        {
            var ids = (await repoUnit.Users.GetByIdAsync(senderId)).FriendRequestsIds;
            var pageList = await repoUnit.Users.GetAllAsync(queryParams, e => e.FriendRequestsIds.Any( id => ids.Contains(id)));
            var responsePageList = new PagedList<FriendResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<FriendResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
            );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<FriendResponseDto>> GetByID(string friendRequestId, string senderId)
        {
            var friendRequest = await repoUnit.FriendRequests.GetByIdAsync(friendRequestId);
            if(friendRequest is null)
                return new() { IsSuccess = false, Errors = ["Invalid Friend Request ID"], StatusCode = MyStatusCode.BadRequest };
            if(friendRequest.SenderId != senderId && friendRequest.ReceiverId != senderId)
                return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden};
            
            var otherId = friendRequest.SenderId == senderId ? friendRequest.ReceiverId : friendRequest.SenderId;
            var user = await repoUnit.Users.GetByIdAsync(otherId);
            
            var responseDto = new FriendResponseDto()
            {
                Id = friendRequestId, UserId = user.Id, Name = user.Name, PictureUrl = user.PictureUrl
            };

            return new() { IsSuccess = true, Data = responseDto, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }
    }
}
