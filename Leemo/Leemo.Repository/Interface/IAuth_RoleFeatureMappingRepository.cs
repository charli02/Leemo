using System;
using System.Collections.Generic;
using TPSS.Common.Interfaces;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Repository.Interface
{
    public interface IAuth_RoleFeatureMappingRepository : IRepository<Auth_RoleFeatureMapping>
    {
        IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> GetAuth_RoleFeaturesByAuth_RoleId(Guid roleId);
        IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> GetAuth_RoleFeaturesByUserId(Guid? userId, Guid? roleId, Guid ProductId);
        int UpdateAuth_RoleFeatureMappingChanges(Guid roleId, Guid userId);


    }
}
