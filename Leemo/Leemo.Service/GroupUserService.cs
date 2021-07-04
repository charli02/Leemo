using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

/// <summary>
/// Represents Leemo service project namespace
/// </summary>
namespace Leemo.Service
{
    /// <summary>
    /// Represnets profile serivce class which interact with repository.
    /// </summary>
    public class GroupUserService : IGroupUserService
    {
        private readonly IGroupUserRepository _groupUserRepository;

        public GroupUserService(IGroupUserRepository groupUserRepository)
        {
            _groupUserRepository = groupUserRepository;
        }

        public void CreateGroupUser(GroupUser groupUser)
        {
            _groupUserRepository.Add(groupUser);
            _groupUserRepository.Save();
        }

        public void InsetGroupUserMapping(List<InputGroupUser> groupUsers, Guid userGroupId)
        {
            _groupUserRepository.InsetGroupUserMapping(groupUsers, userGroupId);
        }
    }
}
