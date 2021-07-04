using System;
using System.Collections.Generic;
using TPSS.Common.Interfaces;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IAuth_RoleFeatureMappingService : IRepository<Auth_RoleFeatureMapping>
    {
        //void CreateProfilePermissionMapping(InputProfilePermissionMapping inputProfilePermissionMapping);
        //Auth_RoleFeatureMapping EditProfilePermissionMapping(Guid id, InputProfilePermissionMapping inputProfilePermissionMapping);
        //IEnumerable<ResultProfliePermissionMapping> GetProfilePermissionsByUserId(Guid userId, string PermissionName = "");

        void CreateAuth_RoleFeatureMapping(InputAuth_RoleFeatureMapping inputProfilePermissionMapping);
        Auth_RoleFeatureMapping EditAuth_RoleFeatureMapping(Guid id, InputAuth_RoleFeatureMapping inputProfilePermissionMapping);
        IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> GetAuth_RoleFeaturesByUserId(Guid? userId, Guid? roleId,Guid ProductId);

        Auth_RoleFeatureMappingTemp GetUniqueRoleFeatureMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp);
        IEnumerable<Auth_RoleFeatureMappingTemp> GetAllRoleFeatureMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp);

        Auth_RoleFeatureMappingTemp InsertAuth_FeatureCodeMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp);
        IEnumerable<Auth_RoleFeatureMappingTemp> UpdateAuth_FeatureCodeMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp);
        IEnumerable<Auth_RoleFeatureMappingTemp> BulkUpdateAuth_FeatureCodeMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp);
        int UpdateAuth_RoleFeatureMappingChanges(Guid roleId, Guid userId);
    }
}
