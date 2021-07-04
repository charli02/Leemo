using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IAuth_RoleUserMappingService
    {
        public IEnumerable<Auth_RoleUserMapping> GetAuth_RoleUserMappingList();

        //public Auth_RoleUserMapping GetAuth_RoleUserMapping(Guid Id);

        public IEnumerable<Auth_RoleUserMapping> GetAuth_RoleUserMappingByUserId(Guid UserId);

        void CreateAuth_RoleUserMapping(Auth_RoleUserMapping auth_RoleUserMapping);
    }
}
