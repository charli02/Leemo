using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using TPSS.Common;
using System.Data;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents profile repository for its CRUD and other custom functions.
    /// </summary>
    public class Auth_RoleUserMappingRepository : RepositoryBase<Auth_RoleUserMapping, LeemoDbContext>, IAuth_RoleUserMappingRepository
    {
        //private LeemoDbContext _context;
        private readonly AppSettings _appSettings;

        public Auth_RoleUserMappingRepository(LeemoDbContext context
            , IOptions<AppSettings> appSettings) : base(context)
        {
            //_context = context;
            _appSettings = appSettings.Value;
        }

        public bool DeleteAuth_RoleUsersMappingByUserId(Guid userId)
        {
            try
            {
                Context.Auth_RoleUserMapping.RemoveRange(Context.Auth_RoleUserMapping.Where(x => x.UserId == userId));
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void InsetAuth_RoleUserMapping(IList<Guid> userRoles, Guid userId)
        {
            Context.Auth_RoleUserMapping.AddRange(
               userRoles.Select(m => new Auth_RoleUserMapping
               {
                   RoleId = Guid.Parse(Convert.ToString(m)),
                   UserId = userId,
                   CreatedOn = DateTime.UtcNow
               }).ToList()
            );
            Context.SaveChanges();
        }

        public bool DeleteAuth_RoleUsersMappingByLocationRolesUserId(Guid companyLocationId,Guid userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
                {
                    DynamicParameters ds = new DynamicParameters();
                    ds.Add("@CompanyLocationId", companyLocationId);
                    ds.Add("@UserId", userId);
                    string query = "[dbo].[sp_DeleteAuth_RoleUsersMappingByUserId]";
                    var result = conn.Query(query, param: ds, commandType: CommandType.StoredProcedure);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
