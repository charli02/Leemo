using System;
using System.Collections.Generic;
using System.Linq;
using TPSS.Common.Implementations;
using Leemo.Data;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

/// <summary>
/// Represents Leemo service project namespace
/// </summary>
namespace Leemo.Service
{
    /// <summary>
    /// This class represents service for profile permission mapping.
    /// </summary>
    public class Auth_RoleFeatureMappingService : RepositoryBase<Auth_RoleFeatureMapping, LeemoDbContext>, IAuth_RoleFeatureMappingService
    {
        private readonly IAuth_RoleFeatureMappingRepository _profilePermissionMappingRepository;
        private readonly IAuth_RoleFeatureMappingTempRepository _auth_RoleFeatureMappingTempRepository;
        private readonly IAuth_RoleFeatureMappingRepository _auth_RoleFeatureMappingRepository;
        public Auth_RoleFeatureMappingService(IAuth_RoleFeatureMappingRepository profilePermissionMappingRepository,
            IAuth_RoleFeatureMappingTempRepository auth_RoleFeatureMappingTempRepository,
            IAuth_RoleFeatureMappingRepository auth_RoleFeatureMappingRepository)
        {
            _profilePermissionMappingRepository = profilePermissionMappingRepository;
            _auth_RoleFeatureMappingTempRepository = auth_RoleFeatureMappingTempRepository;
            _auth_RoleFeatureMappingRepository = auth_RoleFeatureMappingRepository;
        }

        public void CreateAuth_RoleFeatureMapping(InputAuth_RoleFeatureMapping inputProfilePermissionMapping)
        {
            Auth_RoleFeatureMapping profilePermissionMapping = new Auth_RoleFeatureMapping();
            profilePermissionMapping.FeatureId = inputProfilePermissionMapping.FeatureId;
            profilePermissionMapping.RoleId = inputProfilePermissionMapping.RoleId;
            //profilePermissionMapping.AllowInsert = inputProfilePermissionMapping.AllowInsert;
            //profilePermissionMapping.AllowUpdate = inputProfilePermissionMapping.AllowUpdate;
            //profilePermissionMapping.AllowDelete = inputProfilePermissionMapping.AllowUpdate;
            //profilePermissionMapping.AllowExport = inputProfilePermissionMapping.AllowExport;
            //profilePermissionMapping.AllowEmail = inputProfilePermissionMapping.AllowEmail;
            //profilePermissionMapping.AllowDownload = inputProfilePermissionMapping.AllowDownload;
            //profilePermissionMapping.CreatedOn = DateTime.UtcNow;

            _profilePermissionMappingRepository.Add(profilePermissionMapping);
            _profilePermissionMappingRepository.Save();
        }

        public Auth_RoleFeatureMapping EditAuth_RoleFeatureMapping(Guid id, InputAuth_RoleFeatureMapping inputProfilePermissionMapping)
        {
            Auth_RoleFeatureMapping profilePermissionMapping = _profilePermissionMappingRepository.Get(id);

            if (profilePermissionMapping != null)
            {
                profilePermissionMapping.FeatureId = inputProfilePermissionMapping.FeatureId;
                profilePermissionMapping.RoleId = inputProfilePermissionMapping.RoleId;
                profilePermissionMapping.CodeId = profilePermissionMapping.CodeId;
                //profilePermissionMapping.AllowDelete = inputProfilePermissionMapping.AllowDelete;
                //profilePermissionMapping.AllowExport = inputProfilePermissionMapping.AllowExport;
                //profilePermissionMapping.AllowEmail = inputProfilePermissionMapping.AllowEmail;
                //profilePermissionMapping.AllowDownload = inputProfilePermissionMapping.AllowDownload;
                //profilePermissionMapping.CreatedOn = profilePermissionMapping.CreatedOn;
                //profilePermissionMapping.ModifiedOn = DateTime.UtcNow;
            }


            _profilePermissionMappingRepository.Edit(profilePermissionMapping);
            _profilePermissionMappingRepository.Save();

            return profilePermissionMapping;
        }

        public IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> GetAuth_RoleFeaturesByUserId(Guid? userId, Guid? roleId, Guid ProductId)
        {
            return _profilePermissionMappingRepository.GetAuth_RoleFeaturesByUserId(userId, roleId,ProductId);
        }

        public Auth_RoleFeatureMappingTemp GetUniqueRoleFeatureMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp)
        {
            var result = _auth_RoleFeatureMappingTempRepository.GetAllAuth_RoleFeatureMappingTemp()
                .Where(x => x.FeatureId == inputAuth_RoleFeatureMappingTemp.FeatureId &&
                x.CodeId == inputAuth_RoleFeatureMappingTemp.CodeId &&
                x.RoleId == inputAuth_RoleFeatureMappingTemp.RoleId &&
                x.SessionId == inputAuth_RoleFeatureMappingTemp.SessionId).FirstOrDefault();
            return result;
        }

        public IEnumerable<Auth_RoleFeatureMappingTemp> GetAllRoleFeatureMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp)
        {
            var result = _auth_RoleFeatureMappingTempRepository.GetAllAuth_RoleFeatureMappingTemp()
                .Where(x => x.FeatureId == inputAuth_RoleFeatureMappingTemp.FeatureId &&
                x.RoleId == inputAuth_RoleFeatureMappingTemp.RoleId &&
                x.SessionId == inputAuth_RoleFeatureMappingTemp.SessionId).ToList();
            return result;
        }

        public Auth_RoleFeatureMappingTemp InsertAuth_FeatureCodeMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp)
        {
            Auth_RoleFeatureMappingTemp model = new Auth_RoleFeatureMappingTemp();
            if (inputAuth_RoleFeatureMappingTemp != null)
            {
                model.FeatureId = inputAuth_RoleFeatureMappingTemp.FeatureId;
                model.CodeId = inputAuth_RoleFeatureMappingTemp.CodeId;
                model.RoleId = inputAuth_RoleFeatureMappingTemp.RoleId;
                model.CreatedDate = DateTime.UtcNow;
                model.SessionId = inputAuth_RoleFeatureMappingTemp.SessionId;
                model.CreatedBy = inputAuth_RoleFeatureMappingTemp.CreatedBy;
                //model.IsDeleted = false;
                model.IsDeleted = inputAuth_RoleFeatureMappingTemp.IsDeleted;
            }
            _auth_RoleFeatureMappingTempRepository.Add(model);
            _auth_RoleFeatureMappingTempRepository.Save();
            return model;
        }

        public IEnumerable<Auth_RoleFeatureMappingTemp> UpdateAuth_FeatureCodeMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp)
        {
            Auth_RoleFeatureMappingTemp model = GetUniqueRoleFeatureMappingTemp(inputAuth_RoleFeatureMappingTemp);
            if (inputAuth_RoleFeatureMappingTemp != null)
            {
                model.FeatureId = inputAuth_RoleFeatureMappingTemp.FeatureId;
                model.CodeId = inputAuth_RoleFeatureMappingTemp.CodeId;
                model.RoleId = inputAuth_RoleFeatureMappingTemp.RoleId;
                model.CreatedDate = DateTime.UtcNow;
                model.SessionId = inputAuth_RoleFeatureMappingTemp.SessionId;
                model.CreatedBy = inputAuth_RoleFeatureMappingTemp.CreatedBy;
                model.IsDeleted = inputAuth_RoleFeatureMappingTemp.IsDeleted;
            }
            _auth_RoleFeatureMappingTempRepository.Edit(model);
            _auth_RoleFeatureMappingTempRepository.Save();
            return GetAllRoleFeatureMappingTemp(inputAuth_RoleFeatureMappingTemp);
        }

        public IEnumerable<Auth_RoleFeatureMappingTemp> BulkUpdateAuth_FeatureCodeMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp)
        {
            IEnumerable<Auth_RoleFeatureMappingTemp> result = GetAllRoleFeatureMappingTemp(inputAuth_RoleFeatureMappingTemp);
            foreach(var model in result)
            {
                model.FeatureId = inputAuth_RoleFeatureMappingTemp.FeatureId;
                model.CodeId = model.CodeId;
                model.RoleId = inputAuth_RoleFeatureMappingTemp.RoleId;
                model.CreatedDate = DateTime.UtcNow;
                model.SessionId = inputAuth_RoleFeatureMappingTemp.SessionId;
                model.CreatedBy = inputAuth_RoleFeatureMappingTemp.CreatedBy;
                model.IsDeleted = inputAuth_RoleFeatureMappingTemp.IsDeleted;
                _auth_RoleFeatureMappingTempRepository.Edit(model);
                _auth_RoleFeatureMappingTempRepository.Save();
            }
            return result;
        }

        public int UpdateAuth_RoleFeatureMappingChanges(Guid roleId, Guid userId)
        {
           var result = _auth_RoleFeatureMappingRepository.UpdateAuth_RoleFeatureMappingChanges(roleId, userId);
            return result;
        }
    }
}
