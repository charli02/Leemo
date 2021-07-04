//using System;
//using System.Collections.Generic;
//using Leemo.Model;
//using Leemo.Model.Domain;
//using Leemo.Model.ResultModels;
//using Leemo.Repository.Interface;
//using Leemo.Service.Interface;

///// <summary>
///// Represents Leemo service project namespace
///// </summary>
//namespace Leemo.Service
//{
//    /// <summary>
//    /// This class represents service for profile permission.
//    /// </summary>
//    public class PermissionService: IPermissionService
//    {
//        private readonly IPermissionRepository _permissionRepository;
//        private readonly IProfilePermissionMappingRepository _profilePermissionMappingRepository;
//        public PermissionService(IPermissionRepository permissionRepository, IProfilePermissionMappingRepository profilePermissionMappingRepository)
//        {
//            _permissionRepository = permissionRepository;
//            _profilePermissionMappingRepository = profilePermissionMappingRepository;
//        }

//        public IEnumerable<Permission> GetPermissions()
//        {
//            return _permissionRepository.GetAll();
//        }

//        public IEnumerable<ResultProfliePermissionMapping> GetPermissionsByProfileId(Guid proflieId)
//        {
//            return _profilePermissionMappingRepository.GetProfilePermissionsByProfileId(proflieId);
//        }

//        public Permission GetPermission(Guid Id)
//        {
//            return _permissionRepository.Get(Id);
//        }

//        public void CreatePermission(Permission permission)
//        {
//            _permissionRepository.Add(permission);
//            _permissionRepository.Save();
//        }

//        public void EditPermission(Permission permission)
//        {
//            _permissionRepository.Edit(permission);
//            _permissionRepository.Save();
//        }

//        public IEnumerable<Permission> GetPermissionList()
//        {
//            return _permissionRepository.GetAll();
//        }
//    }
//}
