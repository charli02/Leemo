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
    /// Represents group roles repository for its CRUD and other custom functions.
    /// </summary>
    public class GroupRoleRepository : RepositoryBase<GroupDesignationMapping, LeemoDbContext>, IGroupRoleRepository
    {
        //private LeemoDbContext _context;

        public GroupRoleRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public bool DeleteGroupRolesMappingByGroupId(Guid roleGroupId)
        {
            try
            {
                Context.GroupDesignationMapping.RemoveRange(Context.GroupDesignationMapping.Where(x => x.GroupId == roleGroupId));
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void InsetGroupRolesMapping(List<InputGroupRole> groupRoles, Guid roleGroupId)
        {
            Context.GroupDesignationMapping.AddRange(
               groupRoles.Select(m => new GroupDesignationMapping
               {
                   GroupId = roleGroupId,
                   DesignationId = m.RoleId,
                   CreatedOn = DateTime.UtcNow
               }).ToList()
            );
            Context.SaveChanges();
        }
    }
}
