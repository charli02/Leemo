using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IAuth_RoleService
    {
        public IEnumerable<Auth_Role> GetAuth_Roles(Guid CompanyLocationId);

        public Auth_Role GetAuth_Role(Guid Id);

        Auth_Role CreateAuth_Role(Auth_Role auth_Role);

        //void DeleteAuth_Role(Auth_Role auth_Role);
        void DeleteAuth_Role(Auth_Role auth_Role, out int retVal, out string errorMsg);
        void EditAuth_Role(Auth_Role auth_RoleToUpdate, Auth_Role currentAuth_Role);

        IEnumerable<Auth_Role> GetAuth_RoleByUserId(Guid userId, Guid CompanyLocationId);

        IEnumerable<ResultRoleUser> GetAuth_RoleUsersByAuth_RoleId(Guid auth_RoleId);

        IEnumerable<ResultRole> GetAuth_RoleJoinUsers(Guid CompanyLocationId, Guid CompanyId);

        public Auth_Role GetAuth_RoleByName(string name,Guid companyLocationId);

    }
}
