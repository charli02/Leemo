using System;
using TPSS.Common.Interfaces;
using System.Collections.Generic;

using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Repository.Interface
{
    public interface IGroupRepository : IRepository<Group>
    {
        new Group GetById(Guid id);
        new IEnumerable<Group> GetAll();
        IEnumerable<Group> GetActive(Guid companyLocationid);
        IEnumerable<Group> GetInactive(Guid companyLocationid);
        Dictionary<string, int> GetGroupCounts(Guid companyLocationid);
    }
}
