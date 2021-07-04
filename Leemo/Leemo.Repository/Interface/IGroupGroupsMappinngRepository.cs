using System;
using System.Collections.Generic;
using TPSS.Common.Interfaces;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Repository.Interface
{
    public interface IGroupGroupsMappinngRepository : IRepository<GroupGroupsMapping>
    {
        bool DeleteGroupGroupsMappingByGroupId(Guid groupsMappingGroupId);
        void InsetGroupGroupsMapping(List<InputGroupGroupsMapping> groupRoles, Guid groupsMappingGroupId);
    }
}
