using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;
using System.Linq;

namespace Leemo.Service
{
    public class Auth_RoleUserMappingService : IAuth_RoleUserMappingService
    {
        private readonly IAuth_RoleUserMappingRepository _profileUserMappingRepository;
        public Auth_RoleUserMappingService(IAuth_RoleUserMappingRepository profileUserMappingRepository)
        {
            _profileUserMappingRepository = profileUserMappingRepository;
        }

        public void CreateAuth_RoleUserMapping(Auth_RoleUserMapping auth_RoleUserMapping)
        {
            _profileUserMappingRepository.Add(auth_RoleUserMapping);
            _profileUserMappingRepository.Save();
        }

        //public Auth_RoleUserMapping GetAuth_RoleUserMapping(Guid Id)
        //{
        //    return _profileUserMappingRepository.Get(Id);
        //}

        public IEnumerable<Auth_RoleUserMapping> GetAuth_RoleUserMappingByUserId(Guid UserId)
        {
            return _profileUserMappingRepository.GetAll().Where(q => q.UserId == UserId);
        }

        public IEnumerable<Auth_RoleUserMapping> GetAuth_RoleUserMappingList()
        {
            return _profileUserMappingRepository.GetAll();
        }
    }
}
