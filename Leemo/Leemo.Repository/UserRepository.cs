using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using Leemo.Model.InputModels;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents user repository for its CRUD and other custom functions.
    /// </summary>
    public class UserRepository : RepositoryBase<User, LeemoDbContext>, IUserRepository
    {
        //private LeemoDbContext _context;

        public UserRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public new IEnumerable<User> GetAll(Guid CompanyId)
        {
            return Context.User
                .Include(profile => profile.UserProfile)
                    .ThenInclude(z => z.Role)
                        .Where(x => x.UserProfile.CompanyId == CompanyId)
                .ToList();
        }

        public new User GetById(Guid id, Guid CompanyId, Guid CompanyLocationId)
        {
            //return Context.User.Where(x => x.Id == id)
            //    .Include(profile => profile.UserProfile)
            //        .ThenInclude(z => z.Role)
            //        .Where(x => x.UserProfile.CompanyId == CompanyId)
            //    .FirstOrDefault();
            if (CompanyLocationId == Guid.Empty)
            {
                var data = from us in Context.User
                           join clm in Context.CompanyLocationUserMapping on us.Id equals clm.UserId
                           where
                           us.Id == id  &&
                           us.UserProfile.CompanyId == CompanyId 
                           select new User
                           {
                               Id = us.Id,
                               CreatedOn = us.CreatedOn,
                               ForcePasswordReset = us.ForcePasswordReset,
                               IsActive = us.IsActive,
                               isFirstLogin = us.isFirstLogin,
                               ModifiedOn = us.ModifiedOn,
                               UserName = us.UserName,
                               PasswordHash = us.PasswordHash,
                               PasswordSalt = us.PasswordSalt,
                               TempPasswordHash = us.TempPasswordHash,
                               TempPasswordSalt = us.TempPasswordSalt,
                               TempPasswordExpiryDate = us.TempPasswordExpiryDate,
                               UserProfile = us.UserProfile
                           };
                return data.FirstOrDefault();
            }
            else
            {
                var data = from us in Context.User
                           join clm in Context.CompanyLocationUserMapping on us.Id equals clm.UserId
                           where
                           us.Id == id  &&
                           us.UserProfile.CompanyId == CompanyId &&
                           clm.CompanyLocationId == CompanyLocationId
                           select new User
                           {
                               Id = us.Id,
                               CreatedOn = us.CreatedOn,
                               ForcePasswordReset = us.ForcePasswordReset,
                               IsActive = us.IsActive,
                               isFirstLogin = us.isFirstLogin,
                               ModifiedOn = us.ModifiedOn,
                               UserName = us.UserName,
                               PasswordHash = us.PasswordHash,
                               PasswordSalt = us.PasswordSalt,
                               UserProfile = us.UserProfile
                           };
                return data.FirstOrDefault();
            }
        }

        public User GetByUserName(string UserName)
        {
            return Context.User.Where(x => x.UserName == UserName)
                .Include(profile => profile.UserProfile)
                    .ThenInclude(z => z.Role)
                .FirstOrDefault();
        }

        public new IEnumerable<ResultUserByEmailandCompanyID> GetCompanyUsersExceptCurrentCompanyLocation(string email, Guid companyid, Guid companyLocationId)
        {
            var Model = new List<ResultUserByEmailandCompanyID>();
            var data = from us in Context.User
                       join clm in Context.CompanyLocationUserMapping on us.Id equals clm.UserId
                       where
                       us.IsActive == true &&
                       us.UserProfile.CompanyId == companyid &&
                       clm.isBaseLocation == true && us.UserName.Trim().ToLower().Contains(email.Trim().ToLower())
                       select new ResultUserByEmailandCompanyID
                       {
                           Id = us.Id,
                           Email = us.UserName,
                           CompanyLocationId = clm.CompanyLocationId
                       };
            Model = data.Where(x => x.CompanyLocationId != companyLocationId).Take(5).ToList();
            return Model;
        }


        public InputUser GetExistingUserData(Guid UserId, Guid CompanyLocationId)
        {
            var data = from us in Context.User
                       where
                       us.Id == UserId &&
                       us.UserProfile.UserId == UserId

                       select new InputUser
                       {
                           IsActive = us.IsActive,
                           IsExistingUser = true,
                           CompanyLocationId = CompanyLocationId,
                           ExistingUserData = new InputExistingUserData {
                               Id = us.Id,
                               DesignationName = Context.Designation.Where(x => x.Id == us.UserProfile.DesignationId).FirstOrDefault().Name,
                               ReportingToUserEmail = Context.User.Where(x => x.Id == us.UserProfile.ReportingToUserId).FirstOrDefault().UserName
                           },
                           UserName = us.UserName,
                           currentLocationRoles = Context.Auth_Role.Where(x => x.CompanyLocationId == CompanyLocationId)
                                                .Select(xx => new CurrentLocationRoles { ID = xx.Id, Name = xx.Name }).ToList(),
                           userProfile = new InputUserProfile {
                               CompanyId = (Guid)us.UserProfile.CompanyId,
                               Description = us.UserProfile.Description,
                               DesignationId = us.UserProfile.DesignationId,
                               FirstName = us.UserProfile.FirstName,
                               LastName = us.UserProfile.LastName,
                               ReportingToUserId = us.UserProfile.ReportingToUserId,
                           },
                           CompanyLocationName = Context.CompanyLocation.Where(x => x.Id == CompanyLocationId).FirstOrDefault().LocationName
                       };
            return data.FirstOrDefault();
        }

        public new Dictionary<string, int> GetUserCounts(Guid companyLocationid)
        {

            var model = new Dictionary<string, int>();

            var data = from us in Context.User
                       join clm in Context.CompanyLocationUserMapping on us.Id equals clm.UserId
                       where clm.CompanyLocationId == companyLocationid
                       select new ResultUser
                       {
                           UserProfile = us.UserProfile,
                           UserName = us.UserName,
                           Id = us.Id,
                           IsActive = us.IsActive,
                           isFirstLogin = us.isFirstLogin,
                           ForcePasswordReset = us.ForcePasswordReset
                       };



            //var data = Context.CompanyLocationUserMapping.Where(x => x.CompanyLocationId == companyLocationid);
            model.Add("All", data.Count());
            model.Add("Active", data.Where(x => x.IsActive == true).Count());
            model.Add("InActive", data.Where(x => x.IsActive == false).Count());
            return model;
        }
    }
}
