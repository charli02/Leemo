using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IGroupService
    {
        public IEnumerable<Group> GetGroups();

        public ResultGroup GetGroup(Guid Id);

        ResultGroup CreateGroup(InputGroup inputGroup);

        ResultGroup EditGroup(InputGroup inputGroup);

        public Group GetGroupByName(string email,Guid companyLocationId);

        public Group UpdateGroupImage(InputUpdateGroupImage updateGroupImage);

        IEnumerable<Group> GetActiveGroup(Guid companyLocationid);
        IEnumerable<Group> GetInActiveGroup(Guid companyLocationid);
        Dictionary<string, int> GetGroupCounts(Guid companyLocationid);
    }
}
