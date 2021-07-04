using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents group repository for its CRUD and other custom functions.
    /// </summary>
    public class GroupRepository : RepositoryBase<Group, LeemoDbContext>, IGroupRepository
    {

        public GroupRepository(LeemoDbContext context) : base(context)
        {
        }

        public new Group GetById(Guid id)
        {
            return Context.Group.Where(x => x.Id == id)
                .Include(x => x.GroupUser).ThenInclude(x => x.User)
                .Include(x => x.GroupDesignationMapping).ThenInclude(x => x.Designation)
                .Include(x => x.GroupGroupsMapping).ThenInclude(x => x.MappedGroup)
                .ToList()
                .FirstOrDefault();
        }

        public new IEnumerable<Group> GetAll()
        {
            return Context.Group
                .Include(users => users.GroupUser)
                .Include(roles => roles.GroupDesignationMapping)
                .Include(groups => groups.GroupGroupsMapping)
                .ToList();
        }

        public new IEnumerable<Group> GetActive(Guid companyLocationid)
        {
            return Context.Group.Where(x=>x.IsActive == true && x.CompanyLocationId == companyLocationid)
                    .Include(users => users.GroupUser)
                    .Include(roles => roles.GroupDesignationMapping)
                    .Include(groups => groups.GroupGroupsMapping)
                    .ToList();
        }

        public new IEnumerable<Group> GetInactive(Guid companyLocationid)
        {
            return Context.Group.Where(x => x.IsActive == false && x.CompanyLocationId == companyLocationid)
                       .Include(users => users.GroupUser)
                       .Include(roles => roles.GroupDesignationMapping)
                       .Include(groups => groups.GroupGroupsMapping)
                       .ToList();
        }

        public new Dictionary<string , int> GetGroupCounts(Guid companyLocationid) 
        {
            var model = new Dictionary<string , int>();
            var data = Context.Group.Where(x=>x.CompanyLocationId == companyLocationid);
            model.Add("All", data.Count());
            model.Add("Active", data.Where(x => x.IsActive == true).Count());
            model.Add("InActive", data.Where(x => x.IsActive == false).Count());
            return model;
        }


    }
}
