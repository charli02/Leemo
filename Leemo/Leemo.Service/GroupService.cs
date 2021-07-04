using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents Leemo service project namespace
/// </summary>
namespace Leemo.Service
{
    /// <summary>
    /// Represnets profile serivce class which interact with repository.
    /// </summary>
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IGroupGroupsMappinngRepository _groupGroupsMappinngRepository;

        public GroupService(IGroupRepository groupRepository, IGroupUserRepository groupUserRepository, IGroupRoleRepository groupRoleRepository, IGroupGroupsMappinngRepository groupGroupsMappinngRepository)
        {
            _groupRepository = groupRepository;
            _groupUserRepository = groupUserRepository;
            _groupRoleRepository = groupRoleRepository;
            _groupGroupsMappinngRepository = groupGroupsMappinngRepository;
        }

        public IEnumerable<Group> GetGroups()
        {
            return _groupRepository.GetAll();
        }

        public IEnumerable<Group> GetActiveGroup(Guid companyLocationid)
        {
            return _groupRepository.GetActive(companyLocationid);
        }
        public IEnumerable<Group> GetInActiveGroup(Guid companyLocationid)
        {
            return _groupRepository.GetInactive(companyLocationid);
        }

        public Dictionary<string, int> GetGroupCounts(Guid companyLocationid)
        {
            return _groupRepository.GetGroupCounts(companyLocationid);
        }


        public ResultGroup CreateGroup(InputGroup inputGroup)
        {
            Group group = new Group()
            {
                Name = inputGroup.Name,
                Description = inputGroup.Description,
                CreatedOn = DateTime.UtcNow,
                IsActive=true,
                CompanyLocationId = inputGroup.CompanyLocationId
        };
            _groupRepository.Add(group);
            _groupRepository.Save();

            if (group.Id != null && inputGroup.GroupUsers != null)
            {
                _groupUserRepository.InsetGroupUserMapping(inputGroup.GroupUsers, group.Id);
            }

            if (group.Id != null && inputGroup.GroupRoles != null)
            {
                _groupRoleRepository.InsetGroupRolesMapping(inputGroup.GroupRoles, group.Id);
            }

            if (group.Id != null && inputGroup.GroupsMapping != null)
            {
                _groupGroupsMappinngRepository.InsetGroupGroupsMapping(inputGroup.GroupsMapping, group.Id);
            }

            return GetGroup(group.Id);
        }

        public ResultGroup EditGroup(InputGroup inputGroup)
        {
            Group currentGroup = _groupRepository.GetById((Guid)inputGroup.Id);
            currentGroup.Name = inputGroup.Name;
            currentGroup.Description = inputGroup.Description;
            currentGroup.ModifiedOn = DateTime.UtcNow;
            currentGroup.IsActive = inputGroup.IsActive;
            currentGroup.CompanyLocationId = currentGroup.CompanyLocationId;
           
            // update Selected List Of User ,Role And Group
            if (inputGroup.UserIds != null)
            {
                List<GroupUser> groupUsers = new List<GroupUser>();
                foreach (var userids in inputGroup.UserIds)
                {
                    List<GroupUser> users = new List<GroupUser>
                    {
                        new GroupUser{ UserId=userids }
                    };
                    
                    groupUsers.AddRange(users);
                    
                }
                currentGroup.GroupUser = groupUsers;
            }
            if (inputGroup.RoleIds != null)
            {
                List<GroupDesignationMapping> groupRoles = new List<GroupDesignationMapping>();
                foreach (var rolesids in inputGroup.RoleIds)
                {
                    List<GroupDesignationMapping> roles = new List<GroupDesignationMapping>
                    {
                        new GroupDesignationMapping{ DesignationId=rolesids }
                    };
                    
                    groupRoles.AddRange(roles);
                }
                currentGroup.GroupDesignationMapping = groupRoles;
            }
            if (inputGroup.GroupMappingIds != null)
            {
                List<GroupGroupsMapping> groupMapping = new List<GroupGroupsMapping>();
                foreach (var mappinggroupids in inputGroup.GroupMappingIds)
                {
                    List<GroupGroupsMapping> groups = new List<GroupGroupsMapping>
                    {
                        new GroupGroupsMapping{ MappedGroupId= mappinggroupids }
                    };
                    groupMapping.AddRange(groups);
                }
                currentGroup.GroupGroupsMapping = groupMapping;
            }




            _groupRepository.Edit(currentGroup);
            _groupUserRepository.Save();

            if (currentGroup.GroupUser != null && currentGroup.GroupUser.Count > 0)
            {
                if (_groupUserRepository.DeleteGroupUsersMappingByGroupId(currentGroup.Id))
                    _groupUserRepository.InsetGroupUserMapping(inputGroup.GroupUsers, currentGroup.Id);
            }

            if (currentGroup.GroupDesignationMapping != null && currentGroup.GroupDesignationMapping.Count > 0)
            {
                if (_groupRoleRepository.DeleteGroupRolesMappingByGroupId(currentGroup.Id))
                    _groupRoleRepository.InsetGroupRolesMapping(inputGroup.GroupRoles, currentGroup.Id);
            }

            if (currentGroup.GroupGroupsMapping != null && currentGroup.GroupGroupsMapping.Count > 0)
            {
                if (_groupGroupsMappinngRepository.DeleteGroupGroupsMappingByGroupId(currentGroup.Id))
                    _groupGroupsMappinngRepository.InsetGroupGroupsMapping(inputGroup.GroupsMapping, currentGroup.Id);
            }

            return GetGroup(currentGroup.Id);
        }

        public ResultGroup GetGroup(Guid id)
        {
            Group group = _groupRepository.GetById(id);
            ResultGroup resultGroup = new ResultGroup();
            resultGroup.Id = group.Id;
            resultGroup.Name = group.Name;
            resultGroup.ImageName = group.ImageName;
            resultGroup.Description = group.Description;
            resultGroup.CreatedOn = group.CreatedOn;
            resultGroup.ModifiedOn = group.ModifiedOn;
            resultGroup.IsActive = group.IsActive;
            if (group.GroupUser != null && group.GroupUser.Count > 0)
            {
                resultGroup.GroupUsers = new List<ResultGroupUser>();
                foreach (var groupUser in group.GroupUser)
                {
                    ResultGroupUser resultGroupUser = new ResultGroupUser()
                    {
                        UserId = groupUser.User.Id,
                        UserName = groupUser.User.UserName
                    };

                    resultGroup.GroupUsers.Add(resultGroupUser);
                }
            }

            if (group.GroupDesignationMapping != null && group.GroupDesignationMapping.Count > 0)
            {
                resultGroup.GroupRoles = new List<ResultGroupRole>();
                foreach (var groupRole in group.GroupDesignationMapping.Where(x => x.Designation.IsActive == true))
                {
                    ResultGroupRole resultGroupRole = new ResultGroupRole()
                    {
                        RoleId = (Guid)groupRole.Designation.Id,
                        RoleName = groupRole.Designation.Name
                    };

                    resultGroup.GroupRoles.Add(resultGroupRole);
                }
            }

            if (group.GroupGroupsMapping != null && group.GroupGroupsMapping.Count > 0)
            {
                resultGroup.GroupsMapping = new List<ResultGroupGroupsMapping>();
                foreach (var groupGroupsMapping in group.GroupGroupsMapping)
                {
                    ResultGroupGroupsMapping resultGroupGroupsMapping = new ResultGroupGroupsMapping()
                    {
                        MappedGroupId = groupGroupsMapping.MappedGroup.Id,
                        MappedGroupName = groupGroupsMapping.MappedGroup.Name,
                        MappedGroupIsActive=groupGroupsMapping.MappedGroup.IsActive
                    };

                    resultGroup.GroupsMapping.Add(resultGroupGroupsMapping);
                }
            }

            return resultGroup;
        }

        public Group GetGroupByName(string groupName,Guid companyLocationId)
        {
            return _groupRepository.GetAll().Where(x => x.Name.Trim().ToLower() == groupName.Trim().ToLower() && x.CompanyLocationId== companyLocationId).FirstOrDefault();
        }

        public Group UpdateGroupImage(InputUpdateGroupImage updateGroupImage)
        {
            Group currentgroup = _groupRepository.GetById(updateGroupImage.GroupId);
            if (currentgroup != null)
            {
                currentgroup.ImageName = updateGroupImage.ImageName;
                currentgroup.ModifiedOn = DateTime.UtcNow;
                _groupRepository.Edit(currentgroup);
                _groupRepository.Save();
                return currentgroup;
            }
            return currentgroup;
        }
    }
}
