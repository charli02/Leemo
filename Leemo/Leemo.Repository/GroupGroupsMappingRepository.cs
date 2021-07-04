using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Model.InputModels;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents group groups mappinig repository for its CRUD and other custom functions.
    /// </summary>
    public class GroupGroupsMappinngRepository : RepositoryBase<GroupGroupsMapping, LeemoDbContext>, IGroupGroupsMappinngRepository
    {
        //private LeemoDbContext _context;

        public GroupGroupsMappinngRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public bool DeleteGroupGroupsMappingByGroupId(Guid groupsMappingGroupId)
        {
            try
            {
                Context.GroupGroupsMapping.RemoveRange(Context.GroupGroupsMapping.Where(x => x.GroupId == groupsMappingGroupId));
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void InsetGroupGroupsMapping(List<InputGroupGroupsMapping> groupRoles, Guid groupsMappingGroupId)
        {
            Context.GroupGroupsMapping.AddRange(
               groupRoles.Select(m => new GroupGroupsMapping
               {
                   GroupId = groupsMappingGroupId,
                   MappedGroupId = m.MappedGroupId,
                   CreatedOn = DateTime.UtcNow
               }).ToList()
            );
            Context.SaveChanges();
        }
    }
}
