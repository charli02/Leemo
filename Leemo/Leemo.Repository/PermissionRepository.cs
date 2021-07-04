//using Dapper;
//using Microsoft.Extensions.Options;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using TPSS.Common;
//using Leemo.Data;
//using TPSS.Common.Implementations;
//using Leemo.Model;
//using Leemo.Repository.Interface;
//using Leemo.Model.Domain;

///// <summary>
///// Represents Leemo repository project namespace
///// </summary>
//namespace Leemo.Repository.Repository
//{
//    /// <summary>
//    /// Represents permission repository for its CRUD and other custom functions.
//    /// </summary>
//    public class PermissionRepository: RepositoryBase<Permission, LeemoDbContext>, IPermissionRepository
//    {
//        private LeemoDbContext _context;

//        public PermissionRepository(LeemoDbContext context) : base(context)
//        {
//            _context = context;
//        }
//    }
//}
