//using Dapper;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using TPSS.Common;
//using Leemo.Data;
//using TPSS.Common.Implementations;
//using Leemo.Model;
//using Leemo.Model.ResultModels;
//using Leemo.Repository.Interface;
//using Leemo.Model.Domain;

///// <summary>
///// Represents Leemo repository project namespace
///// </summary>
//namespace Leemo.Repository.Repository
//{
//    /// <summary>
//    /// Represents profilepermission mapping repository for its CRUD and other custom functions.
//    /// </summary>
//    public class ProfilePermissionMappingRepository : RepositoryBase<ProfilePermissionMapping, LeemoDbContext>, IProfilePermissionMappingRepository
//    {
//        private LeemoDbContext _context;
//        private readonly AppSettings _appSettings;

//        public ProfilePermissionMappingRepository(LeemoDbContext context, IOptions<AppSettings> appSettings) : base(context)
//        {
//            _context = context;
//            _appSettings = appSettings.Value;
//        }

//        public IEnumerable<ResultProfliePermissionMapping> GetProfilePermissionsByProfileId(Guid proflieId)
//        {
//            using (IDbConnection connection = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
//            {
//                DynamicParameters ds = new DynamicParameters();
//                ds.Add(Constants.SPParameters.ProflieId, proflieId);

//                return connection.Query<ResultProfliePermissionMapping>(
//                    Constants.SP.GetProfilePermissionsByProfileId,
//                    param: ds, commandType:
//                    CommandType.StoredProcedure,
//                    commandTimeout: _appSettings.CommandTimeout
//                );
//            }
//        }

//        public IEnumerable<ResultProfliePermissionMapping> GetProfilePermissionsByUserId(Guid userId, string PermissionName = "")
//        {
//            if (!string.IsNullOrEmpty(PermissionName))
//            {
//                return (from prfliePermission in _context.rolefeatureMapping

//                        join pp in _context.Permission
//                        on prfliePermission.PermissionId equals pp.Id

//                        join profile in _context.Profile
//                        on prfliePermission.ProfileId equals profile.Id

//                        join userPM in _context.ProfileUserMapping
//                        on profile.Id equals userPM.ProfileId

//                        where userPM.UserId == userId
//                        && pp.Name == PermissionName
//                        select new ResultProfliePermissionMapping()
//                        {
//                            ProfileId = profile.Id,
//                            Proflie = profile.Name,
//                            ProfliePermissionId = prfliePermission.Id,
//                            Permission = pp.Name,
//                            AllowDelete = (bool)prfliePermission.AllowDelete,
//                            AllowInsert = (bool)prfliePermission.AllowInsert,
//                            AllowDownload = (bool)prfliePermission.AllowDownload,
//                            AllowEmail = (bool)prfliePermission.AllowEmail,
//                            AllowExport = (bool)prfliePermission.AllowExport,
//                            AllowUpdate = (bool)prfliePermission.AllowUpdate
//                        }
//                              ).ToList();
//            }
//            else
//            {
//                return (from prfliePermission in _context.ProfilePermissionMapping

//                        join pp in _context.Permission
//                        on prfliePermission.PermissionId equals pp.Id

//                        join profile in _context.Profile
//                        on prfliePermission.ProfileId equals profile.Id

//                        join userPM in _context.ProfileUserMapping
//                        on profile.Id equals userPM.ProfileId

//                        where userPM.UserId == userId
//                        select new ResultProfliePermissionMapping()
//                        {
//                            ProfileId = profile.Id,
//                            Proflie = profile.Name,
//                            ProfliePermissionId = prfliePermission.Id,
//                            Permission = pp.Name,
//                            AllowDelete = (bool)prfliePermission.AllowDelete,
//                            AllowInsert = (bool)prfliePermission.AllowInsert,
//                            AllowDownload = (bool)prfliePermission.AllowDownload,
//                            AllowEmail = (bool)prfliePermission.AllowEmail,
//                            AllowExport = (bool)prfliePermission.AllowExport,
//                            AllowUpdate = (bool)prfliePermission.AllowUpdate
//                        }
//                              ).ToList();
//            }
//        }
//    }
//}
