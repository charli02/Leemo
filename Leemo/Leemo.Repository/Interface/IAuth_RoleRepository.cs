using System;
using System.Collections.Generic;
using TPSS.Common.Interfaces;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Repository.Interface
{
    public interface IAuth_RoleRepository : IRepository<Auth_Role>
    {
        IEnumerable<Auth_Role> GetAuth_RolesByUserId(Guid userId, Guid CompanyLocationId);

        IEnumerable<ResultRoleUser> GetAuth_RoleUsersByAuth_RoleId(Guid profileId);

        IEnumerable<Auth_FeatureListWithGeneralCodeResult> GetAuth_FeatureListWithGeneralCodeResults();

    }
}
