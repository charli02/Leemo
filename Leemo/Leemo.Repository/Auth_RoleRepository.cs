using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Model.ResultModels;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using TPSS.Common;
using Microsoft.Extensions.Options;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents profile repository for its CRUD and other custom functions.
    /// </summary>
    public class Auth_RoleRepository : RepositoryBase<Auth_Role, LeemoDbContext>, IAuth_RoleRepository
    {
        //private LeemoDbContext _context;

        private readonly AppSettings _appSettings;

        public Auth_RoleRepository(LeemoDbContext context, IOptions<AppSettings> appSettings) : base(context)
        {
            //_context = context;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<Auth_Role> GetAuth_RolesByUserId(Guid userId, Guid CompanyLocationId)
        {
            var profiles = (from role in Context.Auth_Role
                            join roleMapping in Context.Auth_RoleUserMapping
                            on role.Id equals roleMapping.RoleId                       
                            where roleMapping.UserId == userId && role.CompanyLocationId == CompanyLocationId
                            select role
                          ).ToList();

            return profiles;
        }

        public IEnumerable<ResultRoleUser> GetAuth_RoleUsersByAuth_RoleId(Guid profileId)
        {

            var profileUsers = (from user in Context.User
                            join roleUserMapping in Context.Auth_RoleUserMapping
                            on user.Id equals roleUserMapping.UserId

                            join role in Context.Auth_Role
                            on roleUserMapping.RoleId equals role.Id

                            where role.Id == profileId
                                select new ResultRoleUser() { 
                                     UserId = user.Id,
                                     UserName = string.Concat(user.UserProfile.FirstName, " ", user.UserProfile.LastName),
                                     IsActive = user.IsActive,
                                     Role = user.UserProfile.Role.Name
                                }
                          ).ToList();

            return profileUsers;
        }

        public IEnumerable<Auth_FeatureListWithGeneralCodeResult> GetAuth_FeatureListWithGeneralCodeResults()
        {
            using (SqlConnection conn = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
            {
                DynamicParameters ds = new DynamicParameters();
                //ds.Add("@CompanyID", CommonFunction.sCompanyID);
                //ds.Add("@SvcCode", SvcCode);
                string query = "GetAuth_FeatureListWithGeneralCode";
                var result = conn.Query<Auth_FeatureListWithGeneralCodeResult>(query, param: ds, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }
    }
}
