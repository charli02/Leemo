using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Interfaces;
using Leemo.Model.Domain;

namespace Leemo.Repository.Interface
{
    public interface IAuth_RoleFeatureMappingTempRepository : IRepository<Auth_RoleFeatureMappingTemp>
    {
        IEnumerable<Auth_RoleFeatureMappingTemp> GetAllAuth_RoleFeatureMappingTemp();
    }
}
