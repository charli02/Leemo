using System;
using System.Collections.Generic;
using TPSS.Common.Interfaces;
using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Repository.Interface
{
    public interface IAuth_RoleUserMappingRepository : IRepository<Auth_RoleUserMapping>
    {
        bool DeleteAuth_RoleUsersMappingByUserId(Guid userId);
        bool DeleteAuth_RoleUsersMappingByLocationRolesUserId(Guid companyLocationId, Guid userId);
        void InsetAuth_RoleUserMapping(IList<Guid> userRoles, Guid userId);
    }
}
