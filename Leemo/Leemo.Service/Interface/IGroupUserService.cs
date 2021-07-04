using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IGroupUserService
    {
        void CreateGroupUser(GroupUser groupUser);
        void InsetGroupUserMapping(List<InputGroupUser> groupUsers, Guid userGroupdId);
    }
}
