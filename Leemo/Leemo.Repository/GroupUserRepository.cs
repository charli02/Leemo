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
    /// Represents group users repository for its CRUD and other custom functions.
    /// </summary>
    public class GroupUserRepository : RepositoryBase<GroupUser, LeemoDbContext>, IGroupUserRepository
    {
        //private LeemoDbContext _context;

        public GroupUserRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public bool DeleteGroupUsersMappingByGroupId(Guid userGroupId)
        {
            try
            {
                Context.GroupUser.RemoveRange(Context.GroupUser.Where(x => x.GroupId == userGroupId));
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void InsetGroupUserMapping(List<InputGroupUser> groupUsers, Guid userGroupId)
        {
            Context.GroupUser.AddRange(
               groupUsers.Select(m => new GroupUser
               {
                   GroupId = userGroupId,
                   UserId = m.UserId,
                   CreatedOn = DateTime.UtcNow
               }).ToList()
            );
            Context.SaveChanges();
        }
    }
}
