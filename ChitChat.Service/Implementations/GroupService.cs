﻿using AutoMapper;
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
        public async Task<BaseResult<PagedList<GroupResponseDto>>> GetAllGroups(ItemQueryParams queryParams, string senderId)
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

        public async Task<BaseResult<GroupResponseDto>> GetByID(string groupId, string senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if (group is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };
            if (!group.MembersIds.Contains(senderId) && group.Visability == Visability.Private)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["Group is not Found"] };

            var responseDto = mapper.Map<GroupResponseDto>(group);
            return new() { IsSuccess = true, Data = responseDto, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<GroupResponseDto>> Add(GroupRequestDto dto, string senderId)
        {
            var user = await repoUnit.Users.GetByIdAsync(senderId);
            if (user is null)
                return new() { IsSuccess = false, StatusCode = MyStatusCode.BadRequest, Errors = ["User is not Found"] };
            var group = mapper.Map<Group>(dto);
            group.Id = ObjectId.GenerateNewId().ToString();
            group.AdminsIds.Add(senderId);
            group.MembersIds.Add(senderId);
            group.Permissions = GroupPermissions.AddMembers | GroupPermissions.SendMessages | GroupPermissions.EditGroup;

            user.GroupsIds.Add(group.Id);
            await repoUnit.Users.UpdateAsync(user.Id, user);
            await repoUnit.Groups.AddAsync(group);
            var responseDto = mapper.Map<GroupResponseDto>(group);
            return new() { IsSuccess = true, StatusCode = MyStatusCode.OK, Message = Messages.ADD_SUCCESS, Data = responseDto};
        }

        public async Task<BaseResult<string>> UploadPicture(string groupId, IFormFile image, string senderId)
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

        public async Task<BaseResult<string>> Update(string groupId, GroupRequestDto dto, string senderId)
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

        public async Task<BaseResult<string>> AddMember(string groupId, string userId, string senderId)
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

        public async Task<BaseResult<string>> RemoveMember(string groupId, string memberId, string senderId)
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

        public async Task<BaseResult<string>> AddAdmin(string groupId, string memberId, string senderId)
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

        public async Task<BaseResult<string>> RemoveAdmin(string groupId, string adminId, string senderId)
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

        public async Task<BaseResult<PagedList<UserResponseDto>>> GetAllGroupMembers(string groupId, ItemQueryParams queryParams, string senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if(group is null)
                return new() { IsSuccess = false, Errors = ["Group is not found"], StatusCode = MyStatusCode.BadRequest };

            if (group.Visability == Visability.Private && !group.MembersIds.Contains(senderId))
                return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden };

            var membersIDs = group.MembersIds;

            var pageList = await repoUnit.Users.GetAllAsync(queryParams, u => membersIDs.Contains(u.Id));
            var responsePageList = new PagedList<UserResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<UserResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
            );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<PagedList<UserResponseDto>>> GetAllGroupAdmins(string groupId, ItemQueryParams queryParams, string senderId)
        {
            var group = await repoUnit.Groups.GetByIdAsync(groupId);
            if (group is null)
                return new() { IsSuccess = false, Errors = ["Group is not found"], StatusCode = MyStatusCode.BadRequest };
            
            if(group.Visability == Visability.Private && !group.MembersIds.Contains(senderId))
                return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden};


            var adminsIDs = group.AdminsIds;

            var pageList = await repoUnit.Users.GetAllAsync(queryParams, u => adminsIDs.Contains(u.Id));
            var responsePageList = new PagedList<UserResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<UserResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
            );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<PagedList<GroupResponseDto>>> GetAllUserGroups(ItemQueryParams queryParams, string senderId)
        {
            var user = await repoUnit.Users.GetByIdAsync(senderId);
            if (user is null)
                return new() { IsSuccess = false, Errors = ["User is not found"], StatusCode = MyStatusCode.BadRequest };
            var pageList = await repoUnit.Groups.GetAllAsync(queryParams, g => user.GroupsIds.Contains(g.Id));
            var responsePageList = new PagedList<GroupResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<GroupResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
            );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }
    }
}
