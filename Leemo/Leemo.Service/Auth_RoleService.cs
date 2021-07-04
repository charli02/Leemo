using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;
using Microsoft.Extensions.Options;
using TPSS.Common;

namespace Leemo.Service
{
    public class Auth_RoleService : IAuth_RoleService
    {
        private readonly IAuth_RoleRepository _profileRepository;        
        private readonly ICompanyLocationUserMappingService _companyLocationUserMappingService;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        public Auth_RoleService(IAuth_RoleRepository profileRepository,
            IAuth_RoleFeatureMappingRepository profilePermissionMappingRepository,
            ICompanyLocationUserMappingService companyLocationUserMappingService,
            IUserService userService, IOptions<AppSettings> appSettings)
        {
            _profileRepository = profileRepository;
            _companyLocationUserMappingService = companyLocationUserMappingService;
            _userService = userService;
            _appSettings = appSettings.Value;

        }

        public Auth_Role CreateAuth_Role(Auth_Role inputProfile)
        {
            inputProfile.CreatedOn = DateTime.UtcNow;
            inputProfile.IsDeleted = false;
            _profileRepository.Add(inputProfile);
            _profileRepository.Save();
            return inputProfile;
        }

        public void DeleteAuth_Role(Auth_Role auth_Role, out int retVal, out string errorMsg)
        //public void DeleteAuth_Role(Auth_Role auth_Role)
        {
            //auth_Role.IsDeleted = true;
            //auth_Role.ModifiedOn = DateTime.UtcNow;

            //_profileRepository.Edit(auth_Role);
            //_profileRepository.Save();
            
            using (SqlConnection conn = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
            {
                var reader = conn.QueryMultiple("[dbo].[sp_DeleteRole]", param: new { RoleId = auth_Role.Id }, commandType: CommandType.StoredProcedure);
                var ReturnValue = reader.Read<int>().FirstOrDefault();
                var ErrorMessage = reader.Read<string>().FirstOrDefault();

                retVal = ReturnValue; errorMsg = ErrorMessage.TrimEnd(',') + '.';
            }

        }

        public void EditAuth_Role(Auth_Role profileToUpdate, Auth_Role currentProflie)
        {
            currentProflie.Name = profileToUpdate.Name;
            currentProflie.Description = profileToUpdate.Description; //!= null ? profileToUpdate.Description : currentProflie.Description;
            currentProflie.CreatedOn = currentProflie.CreatedOn;
            currentProflie.CreatedBy = currentProflie.CreatedBy;
            currentProflie.ModifiedBy = profileToUpdate.ModifiedBy;
            currentProflie.ModifiedOn = DateTime.Now;

            _profileRepository.Edit(currentProflie);
            _profileRepository.Save();
        }

        public Auth_Role GetAuth_Role(Guid Id)
        {
            return _profileRepository.Get(Id);
        }

        public Auth_Role GetAuth_RoleByName(string name,Guid companyLocationId)
        {
            return _profileRepository.GetAll().Where(x=>x.Name.Trim().ToLower() == name.Trim().ToLower()&& x.CompanyLocationId == companyLocationId).FirstOrDefault();
        }

        public IEnumerable<Auth_Role> GetAuth_RoleByUserId(Guid userId, Guid CompanyLocationId)
        {
            return _profileRepository.GetAuth_RolesByUserId(userId,CompanyLocationId);
        }

        public IEnumerable<Auth_Role> GetAuth_Roles(Guid CompanyLocationId)
        {
            return _profileRepository.GetAll().Where(x =>  x.IsDeleted == false && x.CompanyLocationId == CompanyLocationId).ToList();
        }

        public IEnumerable<ResultRoleUser> GetAuth_RoleUsersByAuth_RoleId(Guid auth_RoleId)
        {
            return _profileRepository.GetAuth_RoleUsersByAuth_RoleId(auth_RoleId);
        }

        public IEnumerable<ResultRole> GetAuth_RoleJoinUsers(Guid CompanyLocationId, Guid CompanyId)
        {
            var ResultProfiles = GetAuth_Roles(CompanyLocationId);
            var ResultUser = _userService.GetUsers(CompanyId);
            var result = from ResultProfile in ResultProfiles
                         join user1 in ResultUser on ResultProfile.CreatedBy equals user1.UserProfile.UserId into resultuser1
                         from user1 in resultuser1.DefaultIfEmpty()
                         join users2 in ResultUser on ResultProfile.ModifiedBy equals users2.UserProfile.UserId into resultuser2
                         from users2 in resultuser2.DefaultIfEmpty()
                         select new ResultRole
                         {
                             Id = ResultProfile.Id,
                             Name = ResultProfile.Name,
                             Description = ResultProfile.Description,
                             CreatedOn = ResultProfile.CreatedOn,
                             CreatedBy = ResultProfile.CreatedBy,
                             IsDeleted = ResultProfile.IsDeleted,
                             ModifiedOn = ResultProfile.ModifiedOn,
                             ModifiedBy = ResultProfile.ModifiedBy,
                             CreatedByUser = user1 == null ? "" : user1.UserProfile.ImageName == null ? "" : user1.UserProfile.ImageName,
                             ModifiedByUser = users2 == null ? "" : users2.UserProfile.ImageName == null ? "" : users2.UserProfile.ImageName,
                             FirstNameCreatedBy = user1 == null ? "" : user1.UserProfile.FirstName == null ? "" : user1.UserProfile.FirstName,
                             LastNameCreatedBy = user1 == null ? "" : user1.UserProfile.LastName == null ? "" : user1.UserProfile.LastName,
                             FirstNameModifiedBy = users2 == null ? "" : users2.UserProfile.FirstName == null ? "" : users2.UserProfile.FirstName,
                             LastNameModifiedBy = users2 == null ? "" : users2.UserProfile.LastName == null ? "" : users2.UserProfile.LastName,
                         };
            return result.ToList();
        }
    }
}
