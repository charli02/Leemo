using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using TPSS.Common;
using Microsoft.Extensions.Options;
using Dapper;
using System.Linq;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents address type repository for its CRUD and other custom functions.
    /// </summary>
    public class Auth_RoleFeatureMappingRepository : RepositoryBase<Auth_RoleFeatureMapping, LeemoDbContext>, IAuth_RoleFeatureMappingRepository
    {
        //private LeemoDbContext _context;
        private readonly AppSettings _appSettings;

        public Auth_RoleFeatureMappingRepository(LeemoDbContext context, IOptions<AppSettings> appSettings) : base(context)
        {
            //_context = context;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> GetAuth_RoleFeaturesByAuth_RoleId(Guid roleId)
        {
            using (SqlConnection conn = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
            {
                DynamicParameters ds = new DynamicParameters();
                ds.Add("@RoleId", roleId);
                string query = "sp_GetAuth_FeatureListWithGeneralCodeByUserId";
                var result = conn.Query<Auth_FeatureListWithGeneralCodeByUserIdResult>(query, param: ds, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> GetAuth_RoleFeaturesByUserId(Guid? userId, Guid? roleId,Guid ProductId)
        {
            using (SqlConnection conn = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
            {
                DynamicParameters ds = new DynamicParameters();
                ds.Add("@UserId", userId);
                ds.Add("@RoleId", roleId);
                ds.Add("@ProductId", ProductId);
                string query = "[dbo].[sp_GetAuth_FeatureListWithGeneralCodeByUserId_New]";
                var result = conn.Query<Auth_FeatureListWithGeneralCodeByUserIdResult>(query, param: ds, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public int UpdateAuth_RoleFeatureMappingChanges(Guid roleId, Guid userId)
        {
            
            using (SqlConnection conn = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
            {
                DynamicParameters ds = new DynamicParameters();
                ds.Add("@UserId", userId);
                ds.Add("@RoleId", roleId);
                string query = "sp_UpdateAuth_RoleFeatureMappingChanges";
                var result = conn.QueryFirstOrDefault<int>(query, param: ds, commandType: CommandType.StoredProcedure);
                return Convert.ToInt32(result);
            }
        }

    }
}
