using AutoMapper;
using ChitChat.Data.Entities;
using ChitChat.Repository.Helpers;
using ChitChat.Repository.Interfaces;
using ChitChat.Service.DTOs.Request;
using ChitChat.Service.DTOs.Response;
using ChitChat.Service.Helpers;
using ChitChat.Data.Helpers;
using ChitChat.Service.Interfaces;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork repoUnit;
        private readonly IMapper mapper;
        private readonly ICloudService cloudService;

        public GroupService(IUnitOfWork repoUnit, IMapper mapper, ICloudService cloudService)
        {
            this.repoUnit = repoUnit;
            this.mapper = mapper;
            this.cloudService = cloudService;
        }
        public async Task<BaseResult<PagedList<GroupResponseDto>>> GetAll(ItemQueryParams queryParams, ObjectId senderId)
        {
            var pageList = await repoUnit.Groups.GetAllAsync(queryParams, g => g.Visability == Visability.Public || g.MembersIds.Contains(senderId));
            var responsePageList = new PagedList<GroupResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<GroupResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
            );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<GroupResponseDto>> GetByID(ObjectId groupId, ObjectId senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };
            if (!group.MembersIds.Contains(senderId) && group.Visability == Visability.Private)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };

            var responseDto = mapper.Map<GroupResponseDto>(group);
            return new() { IsSuccess = true, Data = responseDto, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<GroupResponseDto>> Add(GroupRequestDto dto, ObjectId senderId)
        {
            var user = await repoUnit.Users.GetByIdAsync(senderId);
            if (user is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["User is not Found"] };
            var group = mapper.Map<Group>(dto);
            group.Id = ObjectId.GenerateNewId();
            group.AdminsIds.Add(senderId);
            group.MembersIds.Add(senderId);
            group.Permissions = GroupPermissions.AddMembers | GroupPermissions.SendMessages | GroupPermissions.EditGroup;

            user.GroupsIds.Add(group.Id);
            await repoUnit.Users.UpdateAsync(user.Id, user);
            await repoUnit.Groups.AddAsync(group);
            var responseDto = mapper.Map<GroupResponseDto>(group);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.ADD_SUCCESS, Data = responseDto};
        }

        public async Task<BaseResult<string>> UploadPicture(ObjectId groupId, IFormFile image, ObjectId senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if(group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };
            if (group.AdminsIds.Contains(senderId))
            {
                var result = await cloudService.UploadImageAsync(image, $"ProfilePicture[{groupId}]");
                var link = result.Data;
                if (!result.IsSuccess || link is null || result.StatusCode != MyStatusCode.OK)
                    return result;
                
                group.PictureUrl = link;

            }else if(group.MembersIds.Contains(senderId))
            {
                var canEdit = (group.Permissions & GroupPermissions.EditGroup) != 0;
                if (!canEdit)
                    return new() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Forbidden"] };

                var result = await cloudService.UploadImageAsync(image, $"ProfilePicture[{groupId}]");
                var link = result.Data;
                if (!result.IsSuccess || link is null || result.StatusCode != MyStatusCode.OK)
                    return result;

                group.PictureUrl = link;
            }else
            {
                return new() { IsSuccess = false, StatusCode = MyStatusCode.Unauthorized, Errors = ["UnAuthenticated"] };
            }
            await repoUnit.Groups.UpdateAsync(groupId, group);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.UPDATE_SUCCESS};
        }

        public async Task<BaseResult<string>> Update(ObjectId groupId, GroupRequestDto dto, ObjectId senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };

            if (group.AdminsIds.Contains(senderId))
            {
                mapper.Map(dto, group);
            }
            else if (group.MembersIds.Contains(senderId))
            {
                var canEdit = (group.Permissions & GroupPermissions.EditGroup) != 0;
                if (!canEdit)
                    return new() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Forbidden"] };

                var permissions = group.Permissions;
                var visibility = group.Visability;

                mapper.Map(dto, group);
                group.Permissions = permissions;
                group.Visability = visibility;
            }
            else
            {
                return new() { IsSuccess = false, StatusCode = MyStatusCode.Unauthorized, Errors = ["UnAuthenticated"] };
            }
            await repoUnit.Groups.UpdateAsync(groupId, group);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.UPDATE_SUCCESS };
        }

        public async Task<BaseResult<string>> AddMember(ObjectId groupId, ObjectId userId, ObjectId senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };
            if (!group.MembersIds.Contains(senderId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };

            if (group.AdminsIds.Contains(senderId))
            {
                group.MembersIds.Add(userId);
            }
            else if (group.MembersIds.Contains(senderId))
            {
                var canAdd = (group.Permissions & GroupPermissions.AddMembers) != 0;
                if (!canAdd)
                    return new() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Forbidden"] };

                group.MembersIds.Add(userId);
            }
            else
            {
                return new() { IsSuccess = false, StatusCode = MyStatusCode.Unauthorized, Errors = ["UnAuthenticated"] };
            }
            await repoUnit.Groups.UpdateAsync(groupId, group);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.UPDATE_SUCCESS };
        }

        public async Task<BaseResult<string>> RemoveMember(ObjectId groupId, ObjectId memberId, ObjectId senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };
            if (!group.AdminsIds.Contains(senderId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };

            if(memberId == senderId)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Already Exist"] };

            group.MembersIds.Remove(memberId);
            group.AdminsIds.Remove(memberId);

            await repoUnit.Groups.UpdateAsync(groupId, group);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.UPDATE_SUCCESS };
        }

        public async Task<BaseResult<string>> AddAdmin(ObjectId groupId, ObjectId memberId, ObjectId senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };
            if (!group.AdminsIds.Contains(senderId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };

            if (memberId == senderId)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Already Exist"] };

            if (!group.MembersIds.Contains(memberId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Member is not Found"] };

            group.AdminsIds.Add(memberId);

            await repoUnit.Groups.UpdateAsync(groupId, group);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.ADD_SUCCESS };
        }

        public async Task<BaseResult<string>> RemoveAdmin(ObjectId groupId, ObjectId adminId, ObjectId senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };
            if (!group.AdminsIds.Contains(senderId))
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };

            if (adminId == senderId)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["You Can not Remove Yourself"] };

            group.AdminsIds.Remove(adminId);

            await repoUnit.Groups.UpdateAsync(groupId, group);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.UPDATE_SUCCESS};
        }
    
    }
}
