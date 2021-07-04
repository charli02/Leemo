using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPSS.Common;
using TPSS.Common.Implementations;
using Leemo.Data;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;

namespace Leemo.Repository
{
    public class Auth_RoleFeatureMappingTempRepository : RepositoryBase<Auth_RoleFeatureMappingTemp, LeemoDbContext>, IAuth_RoleFeatureMappingTempRepository
    {
        //private LeemoDbContext _context;
        private readonly AppSettings _appSettings;

        public Auth_RoleFeatureMappingTempRepository(LeemoDbContext context, IOptions<AppSettings> appSettings) : base(context)
        {
            //_context = context;
            _appSettings = appSettings.Value;
        }


        public IEnumerable<Auth_RoleFeatureMappingTemp> GetAllAuth_RoleFeatureMappingTemp()
        {
            return Context.Auth_RoleFeatureMappingTemp.ToList();
        }
    }
}
