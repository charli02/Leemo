using System;
using System.Collections.Generic;
using System.Linq;
using TPSS.Common.Implementations;
using Leemo.Data;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using Leemo.Repository.Interface;

namespace Leemo.Repository
{
    public class CompanyLocationUserMappingRepository : RepositoryBase<CompanyLocationUserMapping, LeemoDbContext>, ICompanyLocationUserMappingRepository
    {
        public CompanyLocationUserMappingRepository(LeemoDbContext context) : base(context)
        {
        }

        public IEnumerable<CompanyLocationUserMapping> GetAll()
        {
            return Context.CompanyLocationUserMapping.ToList();
        }

        public CompanyLocationUserMapping GetByIds(Guid companyLocationId, Guid userId)
        {
            return Context.CompanyLocationUserMapping.Where(x => x.CompanyLocationId == companyLocationId &&
                                                                x.UserId == userId).FirstOrDefault();
        }

        public IEnumerable<ResultUser> GetUsersByLocation(Guid companyLocationId)
        {
            var data = from us in Context.User
                       join clm in Context.CompanyLocationUserMapping on us.Id equals clm.UserId
                       where clm.CompanyLocationId == companyLocationId
                       select new ResultUser
                       {
                           UserProfile = us.UserProfile,
                           UserName = us.UserName,
                           Id = us.Id,
                           IsActive = us.IsActive,
                           isFirstLogin = us.isFirstLogin,
                           ForcePasswordReset = us.ForcePasswordReset
                       };
            IEnumerable<ResultUser> usersL = data.ToList();
            if (data.Count() > 0)
                usersL.FirstOrDefault().TotalUsers = data.Count();

            return usersL;
        }

        public IEnumerable<CompanyLocation> GetCompanyLocationwithUserId(Guid userId)
        {

            var data = Context.CompanyLocation.Where(x => x.CreatedBy == userId);

            IEnumerable<CompanyLocation> usersL = data.ToList();
            return usersL;
        }

        public IEnumerable<CompanyLocationUserMapping> GetByIds( Guid userId)
        {
            return Context.CompanyLocationUserMapping.Where(x => x.UserId == userId).ToList();
        }
    }
}